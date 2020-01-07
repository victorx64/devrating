# Dev Rating: Software Developer Reward System

## Introduction

Neither the fixed nor the hourly pay of programmers take into account the exact 
volume and quality of work performed. Hourly billing, moreover, motivates the 
contrary to do the work longer. Attempts to measure the programmer’s 
productivity by the number of closed tasks lead to a decrease in the quality of 
the code and an increase in technical debt. The lack of metrics of the quality 
and scope of work of programmers encourages visibility of work and does not 
allow you to see the actual performance of team members. The lack of a unified 
system for tracking the quality of code does not allow programmers to create a 
reputation that would follow them from project to project.

We need a way to determine the reward of a programmer based on the quality and 
size of the written code. The quality of the code would be measured based on 
statistics and would be open. The reward would not take into account the 
personal qualities of the programmer and the preparatory work carried out, but 
only the final result.

## Quality of code

A high-quality code is a code that changes easily. The less code you have to 
remove to meet new requirements, the more maintainable is the codebase.

Thus, each removal of the code is an omission of the programmer who wrote this 
code. The task of the programmer is to minimize possible changes, making the 
codebase better.

Having a history of code changes, we can numerically calculate the quality of 
the programmer's code. Modifying a line is deleting the old, then adding a new 
line.

## Rating system

We introduce a rating system that will show whose code is deleted more often, 
and who more often removes someone else's code. All programmers start with a 
certain rating. Each time someone deletes other people's lines, we will increase 
the rating of the deletor and lower the rating of the author of the deleted line 
in accordance with the Elo formula. If a programmer deletes lines of several 
authors in a work, we first lower the ratings of the authors of the deleted 
lines. Then we increase the programmer’s rating by the sum of the changes of the 
ratings of the authors of the deleted lines.

To reduce the risk of a sharp drop in the rating due to new requirements, the 
programmer is motivated to work on several functionalities or projects in 
parallel.

## Reward

The reward is proportional to the number of new lines. The proportionality 
coefficient depends only on the rating of the performer. Dependence is 
established by the project manager before the start of work.

The reward for work does not take into account the new rating received for this 
work so that other team members have the opportunity to lower this rating until 
receiving the next reward. Thus, the longer the work is done, the more the 
rating can fall.

Each work must go through a code review, which determines the validity of the 
changes. To prevent duplication of code, you need to change the programmers 
working in one piece of code and limit the maximum number of new lines in one 
work.

The reward does not take into account the complexity of the line, which 
motivates decomposing complex logic into several lines. The more uniform the 
complexity of the code, the easier it is to read, and therefore easier to 
change.

The reward does not take into account the difficulty of detecting a bug. If 
significant efforts were required to detect a defective area, then the code 
requires improvements. The programmer can either improve the code himself and 
get a reward for it, or create a task with an indication of a difficult section.

## Conclusion

We got a way to accurately calculate code change rewards. This method motivates 
programmers to understand the business problems of the customer, constantly 
improve the code, write tests and not be tied to one functional. Change of 
programmers will make the project safe from turnover and increase 
maintainability. The rating allows programmers not to lose their reputation 
when switching to another project or company.
