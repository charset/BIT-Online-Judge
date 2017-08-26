'''
methods of file opration in judge
'''
import os
import shutil
import filecmp
import Config

def walkdir(dir,topdown=True):
    fileList = []
    for root, dirs, files in os.walk(dir, topdown):
        for name in files:
            fileList.append(os.path.join(root,name))
    return fileList

def copyfiles(dirSrc, dirDst):
    files = walkdir(dirSrc)
    for f in files:
        os.path.join(dirDst, f.split('/')[-1] )
        shutil.copy2(f, os.path.join(dirDst, f.split('/')[-1] ))

def getdatalist(proid, judgepath):
    '''
    find test data in input/output group via problem id
    move data to judgepath as a copy
    return a list of filename, which describe the test data

    eg.
    list { test1, sample }

    file:
    {
    test1.in
    test1.out

    sample.in
    sample.out
    }

    '''
    datapath = Config.OJ_DATA_PATH + str(proid) + "/"
    copyfiles(datapath, judgepath)
    shutil.copy(Config.OJ_DATA_PATH+"run", judgepath)
    datalist = []

    filelist = os.listdir(judgepath)
    for files in filelist:
        file_name_element = files.split('.')
        if file_name_element[-1] == "in":
            file_name_element[-1] = "out"
            tofind = ""
            for element in file_name_element:
                tofind += element
                tofind += '.'
            tofind = tofind[:-1]

            #if Config.DEBUG:
                #print tofind

            if tofind in filelist:
                datalist.append(tofind[:-4])
    return datalist

def compare(file1, file2):
    '''
    generate answer according to user's outfile and standard outfile
    '''
    file1 = open(file1, "r").read()
    file2 = open(file2, "r").read()
    if file1 == file2:
        return Config.OJ_AC

    cmp1 = ""
    cmp2 = ""
    for i in file1:
        if i != ' ' and i != '\n' and i != '\t':
            cmp1 += i
    for i in file2:
        if i != ' ' and i != '\n' and i != '\t':
            cmp2 += i
    if cmp1 == cmp2:
        return Config.OJ_PE

    return Config.OJ_WA
