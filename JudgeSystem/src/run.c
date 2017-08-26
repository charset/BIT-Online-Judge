/*
author : Reskip
email : Reskiping@gmail.com
Redirect user code

./run ProPath InputFile OutputFile ErrorFile

*/
#include <stdio.h>
#include <unistd.h>
#include <string.h>

int main(int argc, char * argv[])
{
    freopen(argv[2],"r",stdin);
    freopen(argv[3],"w",stdout);
    freopen(argv[4],"a+",stderr);
    strcpy(argv[4],argv[1]);
    execvp( argv[4] , &argv[4] );
    return 0;
}