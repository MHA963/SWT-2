# Charging Station System - README
## Introduction
This charging locker system is designed to facilitate charging of mobile phones and other devices that require a physical connection to a charger. The system works by locking a mobile phone inside a locker where it charges while the user can be away from the phone.

#Usage
The charging locker is intended to be used as follows (assuming the locker is not in use):

1. The user opens the door of the locker.
2. The user connects their mobile phone to the charging cable.
3. The user closes the door of the locker.
4. The user holds their RFID tag up to the system's RFID reader.
5. The system reads the RFID tag. If the charging cable is connected, the system locks the door of the locker and logs the lock. The locker is now occupied and charging begins.

### The user can now leave the locker and come back later. The intended use continues as follows:

6. The user returns to the locker.
7. The user holds their RFID tag up to the system's RFID reader.
8. The system reads the RFID tag. If the RFID matches the one used to lock the locker, charging is stopped, the locker's door is unlocked, and the unlocking is logged.
9. The user opens the locker, removes the charging cable from their phone, and takes their phone out of the locker.
10. The user closes the locker. The locker is now available.

# System Requirements
To use the system, a working RFID reader and a mechanical lock that can be electronically controlled are required. In addition, there must be a connected charging unit to supply power to the devices in the charging Station.

# Additional Information
For further information on using the chargingstation and setting up the system, please refer to the user manual that comes with the system.
