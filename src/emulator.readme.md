Instructions to use emulator
-----------------------------

Right click Project > Properties > Debug Tab

* Choose 'Start external program': Select packages\MessageHandler.Emulator.1.0.0-CIXXX\tools\MessageHandler.Emulator.exe
* Add following command line options:

	/handlerfolder:"pathto\bin\Debug" 
	/inputfolder:"pathto\Input" 
	/outputfolder:"pathto\Output"

* Set working directory: "pathto\bin\Debug"
* Add files to your input folder that contain a raw version of your message (keeping your serialization requirements in mind)
* Hit F5