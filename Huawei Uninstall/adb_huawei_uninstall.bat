@echo 'Start'
@for /f %%a in ('type huawei_package_list.txt') do @adb shell pm uninstall --user 0 %%a
@taskkill /f /t /im adb.exe
@pause