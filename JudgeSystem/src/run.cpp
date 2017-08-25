/*
author : Reskip
email : Reskiping@gmail.com
Redirect user code

./run ProPath InputFile OutputFile ErrorFile

*/
#include <stdio.h>
#include <unistd.h>

int main(int argc, char * argv[])
{
    freopen(argv[2],"r",stdin);
    freopen(argv[3],"w",stdout);
    freopen(argv[4],"w",stderr);
    char * arg[] = { argv[1] };
    execvp( arg[0] , arg );
    return 0;
}