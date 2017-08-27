'''
Compile methods
'''
import os
import Config

CP_C = ("./run", "gcc", "in.in", "out.out", Config.OJ_ERR_FILE, "main.c", "-o", "main",
        "-fno-asm", "-Wall", "-lm", "--static", "-std=c99", "-DONLINE_JUDGE")
CP_CC = ("./run", "g++", "in.in", "out.out", Config.OJ_ERR_FILE, "main.cc", "-o", "main",
         "-fno-asm", "-Wall", "-lm", "--static", "-std=c++0x", "-DONLINE_JUDGE")

def comp(lang):
    '''
    Compile source file accroding to language set
    '''
    if Config.LANGSET[lang] == "c":
        os.execvp(CP_C[0], CP_C)
    if Config.LANGSET[lang] == "cc":
        os.execvp(CP_CC[0], CP_CC)
