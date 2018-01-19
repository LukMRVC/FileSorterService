# File Sorter
Service that watches downloads folders and connected with FileSorterOptions, sorts downloaded files to according destination folders.
This is a windows service/WPF Project that is only Windows compatible.

You can download [setup here.](https://github.com/LukMRVC/FileSorterService/blob/master/File%20Sorter%20Setup/Release/File%20Sorter%20Setup.msi?raw=true)
## This project consists of 2 parts:
### File Sorter Serivce
A service that watches the downloads folder a sortes files into directories, 
based on options which the user made and saved in File Sorter Options.

### File Sorter Options 
File Sorter Options required administration rights, because otherwise cannot start and stop the service.
You can add directories, delete them, batch sort or start a service.
Installer donwload will be added.
