from pathlib import Path
from shutil import copy2, make_archive, rmtree


PATH_ASSETBUNDLES = Path("../Assets/AssetBundles")
PATH_BINARIES = Path("../Logviewer/bin/Debug")
PATH_PACKAGE = Path("000_Logviewer")

# Make a temporary directory, all other files will be copied to this
# directory so that the directory can be zipped in its entirety by shutil
PATH_PACKAGE.mkdir()

# Copy files to the temporary directory
copy2(PATH_ASSETBUNDLES / "windows/logvieweruiassets", PATH_PACKAGE)
copy2(PATH_BINARIES / "Logviewer.dll", PATH_PACKAGE)
copy2(PATH_BINARIES / "Logviewer.Unity.dll", PATH_PACKAGE)
copy2(Path("license_distribute.txt"), PATH_PACKAGE)
(PATH_PACKAGE / "license_distribute.txt").rename(PATH_PACKAGE / "license.txt")

# Package for windows
make_archive("Logviewer_windows", "zip", Path(), PATH_PACKAGE)

# Replace logvieweruiassets with the one built for linux, and package
Path(PATH_PACKAGE, "logvieweruiassets").unlink()
copy2(PATH_ASSETBUNDLES / "linux/logvieweruiassets", PATH_PACKAGE)
make_archive("Logviewer_linux", "zip", Path(), PATH_PACKAGE)

# Replace heatmapuiassets with the one built for osx, and package
Path(PATH_PACKAGE, "logvieweruiassets").unlink()
copy2(PATH_ASSETBUNDLES / "linux/logvieweruiassets", PATH_PACKAGE)
make_archive("Logviewer_osx", "zip", Path(), PATH_PACKAGE)

# Remove Logviewer folder and subfolders
rmtree(PATH_PACKAGE)
