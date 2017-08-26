'''

'''
import Config
import os

CP_C = ("./run" ,"gcc" ,"in.in" ,"out.out" ,Config.OJ_ERR_FILE ,"main.c", "-o", "main", "-fno-asm", "-Wall", "-lm", "--static", "-std=c99", "-DONLINE_JUDGE")


def compile(lang):
    if Config.LANGSET[lang] == "c":
        os.execvp(CP_C[0], CP_C)

