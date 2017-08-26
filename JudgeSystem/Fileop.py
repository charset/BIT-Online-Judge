'''
methods of file opration in judge
'''
import os
import shutil
import Config

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
    shutil.copytree(datapath, judgepath)

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

            if Config.DEBUG:
                print tofind

            if tofind in filelist:
                datalist.append(tofind[:-4])
    return datalist

def 