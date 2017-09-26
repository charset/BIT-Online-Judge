'''
configuration of online judge system

'''

'''
copy from HUST
I don't understand some of them, but i think it will all have some use...
'''
DEBUG = 0

OJ_WT0 = 0
OJ_WT1 = 1
OJ_CI = 2
OJ_RI = 3
OJ_AC = 4
OJ_PE = 5
OJ_WA = 6
OJ_TL = 7
OJ_ML = 8
OJ_OL = 9
OJ_RE = 10
OJ_CE = 11
OJ_CO = 12

LANGSET = {
    0 : "c",
    1 : "cc",
    2 : "pas",
    3 : "java",
    4 : "rb",
    5 : "sh",
    6 : "py",
    7 : "php",
    8 : "pl",
    9 : "cs",
    10 : "m",
    11 : "bas",
    12 : "scm"
}

REQUEST_URL = "http://127.0.0.1:5000"
UPDATE_URL = "http://127.0.0.1:5000"
OJ_JUDGE_PATH = "home/"
OJ_DATA_PATH = "data/"
OJ_ERR_FILE = "error.txt"
OJ_OUTPUT_FILE = "userout.txt"
OJ_PATH_ROOT = "/home/reskip/Desktop/BIT-Online-Judge/"

USER_JUDGE = 1001

STD_MB = 1048576 # 2^20  byte/MB
STD_KB = 1024   #2^10 byte/KB
OJ_COMPILE_MEM = 1024 * STD_MB # memory limit in compile /byte
OJ_COMPILE_TIME = 10 # time limit in compile /s
OJ_COMPILE_FSIZE = 32 * STD_MB # file size /byte
OJ_FSIZE_LIMIT = 32 * 1024 #output file size /KB

OJ_THREAD = 4

def getpassword():
    return 0
