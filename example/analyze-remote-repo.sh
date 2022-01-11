#!/bin/bash

set -eo pipefail
set -eo pipefail && VERBOSE='-v' # verbose mode
# set -eox pipefail && VERBOSE='-v' # debug mode

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
NC='\033[0m' # No color

# --------------------

dotnet build ../consoleapp/devrating.consoleapp.csproj
echo

DEVRATING=../consoleapp/bin/Debug/net6.0/devrating.consoleapp
echo 'devrating version' $($DEVRATING --version)
echo 'dotnet version' $(dotnet --version)
git --version
echo

git submodule update --init -- ./glowing-adventure/
git -C ./glowing-adventure/ fetch # to fetch tags
git -C ./glowing-adventure/ --no-pager log --pretty='%h `%s` %ae' --graph
echo

# --------------------

rm -f ./devrating.sqlite3
rm -f ./devrating.sqlite3.journal

# --------------------

function add_main_branch_commit {
	printf "${GREEN}Add merge commit $1 with$(git -C ./glowing-adventure/ diff  --shortstat -w $1~..$1)${NC}\n"
	$DEVRATING add commit --merge $1 --path ./glowing-adventure/ --branch main $VERBOSE
	echo

	printf "${YELLOW}Minimal PR sizes${NC}\n"
	$DEVRATING top
	echo
}

# --------------------

add_main_branch_commit 29df664
add_main_branch_commit fc4620c # (tag: v0.1.0)
add_main_branch_commit 318a516 # (tag: v1.0.0)
add_main_branch_commit 3ec7ac8
add_main_branch_commit a06751a
add_main_branch_commit b9b6bc7
