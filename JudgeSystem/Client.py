'''
#TODO
'''
# -*- coding:utf-8 -*-

import os
import json
import resource
import psutil
import signal
import sys
import math

import Config
import Fileop
import Compile

class Judge(object):
    '''
    #TODO
    '''
    runid = ""
    proid = ""
    lang = 0
    mlimit = 0 #kB
    tlimit = 0  #ms
    flimit = 0  #kB
    spj = 0 # 0 No / 1 Yes
    code = ""

    status = Config.OJ_AC
    exetime = 0 #ms
    exemem = 0 #MB
    errinfo = ""

    def __init__(self, JsonPkg):
        judgeinfo = json.JSONDecoder().decode(JsonPkg)

        self.runid = judgeinfo['runid']
        self.proid = judgeinfo['proid']
        self.lang = int(judgeinfo['lang'])
        self.mlimit = int(judgeinfo['mlimit'])
        self.tlimit = int(judgeinfo['tlimit'])
        self.flimit = Config.OJ_FSIZE_LIMIT
        self.spj = int(judgeinfo['spj'])
        self.code = judgeinfo['code']

    def compile(self):
        '''compile and try to exec'''
        judgepath = Config.OJ_JUDGE_PATH + "/" + self.runid

        if Config.DEBUG:
            print judgepath

        source = open(judgepath + "/main." + Config.LANGSET[self.lang], "w")
        source.write(self.code)
        source.close()

        pid = os.fork()
        if pid == 0:
            os.chroot(judgepath)
            os.seteuid(Config.USER_JUDGE)
            os.nice(100)

            resource.setrlimit(resource.RLIMIT_AS, (Config.OJ_COMPILE_MEM, Config.OJ_COMPILE_MEM))
            resource.setrlimit(resource.RLIMIT_CPU, (Config.OJ_COMPILE_TIME, Config.OJ_COMPILE_TIME))
            resource.setrlimit(resource.RLIMIT_FSIZE, (Config.OJ_COMPILE_FSIZE, Config.OJ_COMPILE_FSIZE))

            Compile.compile(self)
            exit(0)
        else:
            res = os.waitpid(pid)[1]
            if Config.DEBUG:
                print "compile status : ",
                print res
            return res


    def execute(self, dataname):
        '''
        execute data named dataname and monitor runtime error
        '''
        judgepath = Config.OJ_JUDGE_PATH + "/" + self.runid + "/"
        os.chdir(judgepath)

        inputfile = dataname
        outputfile = Config.OJ_OUTPUT_FILE
        errorfile = Config.OJ_ERR_FILE
        proname = "./main"  #TODO

        pid = os.fork()
        if pid == 0:
            os.chroot(judgepath)
            os.seteuid(Config.USER_JUDGE)
            os.nice(100)

            resource.setrlimit(resource.RLIMIT_AS, (Config.STD_KB * self.mlimit, Config.STD_KB * self.mlimit))
            resource.setrlimit(resource.RLIMIT_CPU, (math.ceil(self.tlimit/1000) , math.ceil(self.tlimit/1000)))
            resource.setrlimit(resource.RLIMIT_FSIZE, (self.flimit * Config.STD_KB, self.flimit * Config.STD_KB))

            os.execvp("./run", ("./run", proname, inputfile, outputfile, errorfile))
        else:
            status = 0
            fatpid = os.getpid()
            while 1:
                p = psutil.Process(pid)
                fat = psutil.Process(fatpid)
                if p.status() == "zombie":
                    break
                self.exetime = p.cpu_times()[0] * 1000
                self.exemem = max(self.exemem, p.memory_info()[1] / Config.STD_KB)
                if self.exetime > self.tlimit or fat.cpu_times()[0]*1000 > self.tlimit * 10:
                    self.status = Config.OJ_TL
                    os.kill(pid, signal.SIGKILL)
                    return
                if self.exemem > self.mlimit:
                    self.status = Config.OJ_ML
                    os.kill(pid, signal.SIGKILL)
                    return
            status = os.waitpid(pid)[1] & 31    #produce signal code from exit code

            if status == signal.SIGCHLD or status == signal.SIGALRM or status == signal.SIGXCPU or status == signal.SIGKILL:
                self.status = Config.OJ_TL
                return
            if status == signal.SIGXFSZ:
                self.status = Config.OJ_OL
                return
            if os.WIFEXITED(status):
                return
            self.status = Config.OJ_RE
            return

    def compare(self, dataname):
        pass

    def judge(self):
        '''
        run this method while judge information has already prepared
        use judge info and problem data to get solution and update response
        '''
        judgepath = Config.OJ_JUDGE_PATH + str(self.runid) + "/"

        ret = compile()
        if ret != 0:
            self.status = Config.OJ_CE
            self.errinfo = open(judgepath + Config.OJ_ERR_FILE, "r").read()
            return

        datalist = Fileop.getdatalist(self.proid, judgepath)

        for dataname in datalist:
            self.execute(dataname)
            if self.status != Config.OJ_AC and self.status != Config.OJ_PE:
                break
            self.compare(dataname)
            if self.status != Config.OJ_AC and self.status != Config.OJ_PE:
                break
        return


class Client:
    
    pass