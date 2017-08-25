'''
#TODO
'''
# -*- coding:utf-8 -*-

import os
import json
import resource
import signal
import sys
import config
import fileop
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

    status = config.OJ_WT0
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
        self.spj = int(judgeinfo['spj'])
        self.code = judgeinfo['code']

    def compile(self):
        '''compile and try to exec'''
        judgepath = config.OJ_JUDGE_PATH + "/" + self.runid

        if config.DEBUG:
            print judgepath

        source = open(judgepath + "/main." + config.LANGSET[self.lang], "w")
        source.write(self.code)
        source.close()

        pid = os.fork()
        if pid == 0:
            os.chroot(judgepath)
            os.seteuid(config.USER_JUDGE)
            os.nice(100)

            resource.setrlimit(resource.RLIMIT_AS, (config.OJ_COMPILE_MEM, config.OJ_COMPILE_MEM))
            resource.setrlimit(resource.RLIMIT_CPU, (config.OJ_COMPILE_TIME, config.OJ_COMPILE_TIME))
            resource.setrlimit(resource.RLIMIT_FSIZE, (config.OJ_COMPILE_FSIZE, config.OJ_COMPILE_FSIZE))

            Compile.compile(self)
            exit(0)
        else:
            res = os.waitpid(pid)[1]
            if config.DEBUG:
                print "compile status : ",
                print res
            return res


    def execute(self, dataname):
        judgepath = config.OJ_JUDGE_PATH + "/" + self.runid
        
        pid = os.fork()
        if pid == 0:
            os.chroot(config.OJ_JUDGE_PATH)
            os.seteuid(config.USER_JUDGE)
            os.nice(100)


        else:
            return pid



    def compare(self, judgeid, dataname):
        pass

    def judge(self, judgeid):
        '''
        run this method while judge information has already prepared
        use judge info and problem data to get solution and update response
        '''
        ret = compile(judgeid)
        if ret != 0:
            self.status = config.OJ_CE
            self.errinfo = open(config.OJ_JUDGE_PATH + "/ce.txt", "r").read()
            return

        datalist = fileop.getdatalist(self.proid, judgeid)

        for dataname in datalist:
            self.run(judgeid, dataname)
            if self.status != config.OJ_AC and self.status !=config.OJ_PE:
                break
            self.compare(judgeid, dataname)
            if self.status != config.OJ_AC and self.status != config.OJ_PE:
                break

        return


class Client:
    
    pass