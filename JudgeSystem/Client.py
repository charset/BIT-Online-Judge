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
import time
import math
import json
import urllib2
import shutil
import threading
from Queue import Queue

import Config
import Fileop
import Compile


workQueue = Queue()
updateQueue = Queue()
threadNumber = 0

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
    isEnd = 0

    def __init__(self, judgeinfo):
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
        judgepath = Config.OJ_PATH_ROOT + Config.OJ_JUDGE_PATH + "/" + self.runid

        source = open(judgepath + "/main." + Config.LANGSET[self.lang], "w")
        source.write(self.code)
        source.close()

        pid = os.fork()
        if pid == 0:
            #os.chroot(judgepath)
            #os.seteuid(Config.USER_JUDGE)
            #os.nice(100)
            os.chdir(judgepath)
            if Config.DEBUG:
                print "compile path : ",
                print os.getcwd()
            resource.setrlimit(resource.RLIMIT_AS, (Config.OJ_COMPILE_MEM, Config.OJ_COMPILE_MEM))
            resource.setrlimit(resource.RLIMIT_CPU, (Config.OJ_COMPILE_TIME, Config.OJ_COMPILE_TIME))
            resource.setrlimit(resource.RLIMIT_FSIZE, (Config.OJ_COMPILE_FSIZE, Config.OJ_COMPILE_FSIZE))

            Compile.compile(self.lang)
            exit(0)
        else:
            res = os.waitpid(pid, 0)[1]
            if Config.DEBUG:
                print "compile status : ",
                print res
            return res


    def execute(self, dataname):
        '''
        execute data named dataname and monitor runtime error
        '''
        judgepath = Config.OJ_PATH_ROOT + Config.OJ_JUDGE_PATH + self.runid + "/"

        if Config.DEBUG:
            print "execute : ",
            print " dataname = " + dataname,
            print " exepath = " + os.getcwd()

        inputfile = dataname
        outputfile = Config.OJ_OUTPUT_FILE
        errorfile = Config.OJ_ERR_FILE
        proname = "./main"  #TODO

        pid = os.fork()
        if pid == 0:
            #print os.getcwd()
            
            #os.chroot(".")
            #os.seteuid(Config.USER_JUDGE)
            #os.nice(100)
            os.chdir(judgepath)

            resource.setrlimit(resource.RLIMIT_AS, (Config.STD_KB * self.mlimit, Config.STD_KB * self.mlimit))
            resource.setrlimit(resource.RLIMIT_CPU, (math.ceil(self.tlimit/1000.0) , math.ceil(self.tlimit/1000.0)))
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
            status = os.waitpid(pid, 0)[1] & 31    #produce signal code from exit code

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
        '''
        compare user's output and update judge status
        '''
        judgepath = Config.OJ_PATH_ROOT + Config.OJ_JUDGE_PATH + "/" + self.runid
        #os.chdir(judgepath)
        if Config.DEBUG:
            print "compare : datapath = ",
            print os.getcwd()

        inputfile = judgepath + dataname + ".in"
        useroutfile = judgepath + Config.OJ_OUTPUT_FILE
        stdoutfile = judgepath + dataname + ".out"
        
        if self.spj:
            pass
        else:
            self.status = max(self.status, Fileop.compare(useroutfile, stdoutfile))
        return

    def printstatus(self):
        '''
        print status
        '''
        print " runid: " + self.runid,
        print " status: " + str(self.status),
        print " memory: " + str(self.exemem),
        print " time: " + str(self.exetime)
        print " error: " + self.errinfo

    def judge(self):
        '''
        run this method while judge information has already prepared
        use judge info and problem data to get solution and update response
        '''
        judgepath = Config.OJ_PATH_ROOT + Config.OJ_JUDGE_PATH + str(self.runid) + "/"
        if os.path.exists(judgepath):
            shutil.rmtree(judgepath)
        os.makedirs(judgepath)
        shutil.copy(Config.OJ_DATA_PATH + "run",judgepath)

        ret = self.compile()
        os.chdir(Config.OJ_PATH_ROOT)
        if ret != 0:
            self.status = Config.OJ_CE
            self.errinfo = open(judgepath + Config.OJ_ERR_FILE, "r").read()
            shutil.rmtree(judgepath)
            self.isEnd = 1
            return

        datalist = Fileop.getdatalist(self.proid, judgepath)
        if Config.DEBUG:
            print " datalist : ",
            print datalist
        for dataname in datalist:
            self.execute(dataname + ".in")
            if self.status != Config.OJ_AC and self.status != Config.OJ_PE:
                break
            self.compare(dataname)
            if self.status != Config.OJ_AC and self.status != Config.OJ_PE:
                break

        #shutil.rmtree(judgepath)
        self.isEnd = 1
        return


class Runer(threading.Thread):
    '''
    Insert a new thread to run judge
    '''
    def run(self):
        global workQueue
        global updateQueue
        global threadNumber
        if workQueue.qsize() == 0:
            threadNumber -= 1
            return
        work = workQueue.get()
        work.judge()
        updateQueue.put(work)
        threadNumber -= 1
        return
        

class Client(object):
    '''
    TODO
    '''
    global workQueue
    global updateQueue

    def __init__(self):
        os.setuid(0)

    def getsubmit(self):
        '''
        Get the submission information from the client server
        '''
        data = urllib2.urlopen(Config.REQUEST_URL).read()
        if Config.DEBUG:
            print "jsondata = ",
            print data
        jsondata = json.loads(data)
        if int(jsondata["runid"]) != -1:
            workQueue.put(Judge(jsondata))
            return 1
        return 0

    def update(self):
        '''
        update judge info to client server
        '''
        print updateQueue.qsize()
        while updateQueue.qsize() > 0:
            update = updateQueue.get()
            jsondata = json.dumps({
                "runid" : update.runid,
                "status" : update.status,
                "exetime" : update.exetime,
                "exemem" : update.exemem,
                "errinfo" : update.errinfo,
                "password" : Config.getpassword()
            })
            update.printstatus()
        return

    def work(self):
        '''
        work
        '''
        global threadNumber

	while True:
	    while self.getsubmit():
	        pass

	    for i in range(Config.OJ_THREAD - threadNumber):
	        runer = Runer()
	        threadNumber += 1
	        runer.start()

   	    self.update()
	    time.sleep(1)
            print "round End"

if __name__ == "__main__":
    client = Client()
    client.work()
