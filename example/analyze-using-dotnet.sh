#!/bin/bash

set -eo pipefail
# set -eo pipefail && VERBOSE='-v' # verbose mode
# set -eox pipefail && VERBOSE='-v' # debug mode

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NO_COLOR='\033[0m'

# --------------------

if [ -f ./devrating.sqlite3 ]; then
	echo './devrating.sqlite3 exists'
	exit 1
fi

# --------------------

dotnet build ../consoleapp/devrating.consoleapp.csproj
echo

# --------------------

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

function add_main_branch_commit {
	printf "${GREEN}Add merge commit $1 with$(git -C ./glowing-adventure/ diff  --shortstat -w $1~..$1)${NO_COLOR}\n"
	$DEVRATING add commit --merge $1 --path ./glowing-adventure/ --branch main $VERBOSE
	echo

	printf "${YELLOW}Minimal PR sizes${NO_COLOR}\n"
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
