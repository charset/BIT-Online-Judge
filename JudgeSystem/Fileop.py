'''
methods of file opration in judge
'''
import os
import shutil
import config

def getdatalist(proid, judgeid):
    '''
    find test data in input/output group via problem id
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
    datapath = config.OJ_DATA_PATH + "/" + str(proid)
    judgepath = config.OJ_JUDGE_PATH + "/" + str(judgeid) + "/data"
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

            if config.DEBUG:
                print tofind

            if tofind in filelist:
                datalist.append(tofind[:-4])
    return datalist
