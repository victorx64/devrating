#!/bin/bash

set -eo pipefail

BLACK='\033[0;30m'
RED='\033[0;31m'
GREEN='\033[0;32m'
ORANGE='\033[0;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
LGRAY='\033[0;37m'
DGRAY='\033[1;30m'
LRED='\033[1;31m'
LGREEN='\033[1;32m'
YELLOW='\033[1;33m'
LBLUE='\033[1;34m'
LPURPLE='\033[1;35m'
LCYAN='\033[1;36m'
WHITE='\033[1;37m'
NC='\033[0m' # No Color

# --------------------

dotnet build ../consoleapp/devrating.consoleapp.csproj
echo

DEVRATING=../consoleapp/bin/Debug/net6.0/devrating.consoleapp
echo 'devrating version' $($DEVRATING --version)
echo 'dotnet version' $(dotnet --version)
git --version
echo

git submodule update --init -- ./glowing-adventure/
git -C ./glowing-adventure/ log --pretty='%h `%s` %ae' --graph
echo

# --------------------

rm -f ./devrating.sqlite3
rm -f ./devrating.sqlite3.journal

# --------------------
printf "${GREEN}Add E merge commit${NC}\n"

git -C ./glowing-adventure/ diff -w -U0 318a516~..318a516
echo

$DEVRATING add commit --merge 318a516 --path ./glowing-adventure/ --branch main
