cd bin/debug/netcoreapp2.2

dotnet LotteryStatistics.Console.dll /run=ShowNumberStatistics,ShowBonusBallStatistics,ShowPatternStatistics,ShowGeneratedNumbers,ShowExistingLotteryNumbersPerPattern /lotteries=Powerball,"Mega Millions" /OutputMode=File /NumberOfRandomSelections=2 /pattern="1,2,3,4,5"
@echo DONE
@echo off
rem Show Number Statistics for all lotteries
rem dotnet LotteryStatistics.Console.dll /run=ShowNumberStatistics /lotteries=Powerball /OutputMode=File
rem dotnet LotteryStatistics.Console.dll /run=ShowNumberStatistics /lotteries="Mega Millions" /OutputMode=File

rem Show Bonus Ball Statistics for all lotteries
rem dotnet LotteryStatistics.Console.dll /run=ShowBonusBallStatistics /lotteries=Powerball /OutputMode=File
rem dotnet LotteryStatistics.Console.dll /run=ShowBonusBallStatistics /lotteries="Mega Millions" /OutputMode=File

rem Show Pattern Statistics for all Lotteries
rem dotnet LotteryStatistics.Console.dll /run=ShowPatternStatistics /lotteries=Powerball /OutputMode=File
rem dotnet LotteryStatistics.Console.dll /run=ShowPatternStatistics /lotteries="Mega Millions" /OutputMode=File

rem Show Random Numbers based on all lotteries
rem dotnet LotteryStatistics.Console.dll /run=ShowGeneratedNumbers /lotteries=Powerball /OutputMode=File /NumberOfRandomSelections=2
rem dotnet LotteryStatistics.Console.dll /run=ShowGeneratedNumbers /lotteries="Mega Millions" /OutputMode=File /NumberOfRandomSelections=2

rem Show Numbers based on all lotteries
rem dotnet LotteryStatistics.Console.dll /run=ShowExistingLotteryNumbersPerPattern /lotteries=Powerball /OutputMode=File /pattern="1,2,3,4,5"
rem dotnet LotteryStatistics.Console.dll /run=ShowExistingLotteryNumbersPerPattern /lotteries="Mega Millions" /OutputMode=File /pattern="1,2,3,4,5"

cd ../../../