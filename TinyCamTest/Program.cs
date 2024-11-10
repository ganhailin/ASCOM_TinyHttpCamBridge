﻿// This is a console application that can be used to test an ASCOM driver

// Remove the "#define UseChooser" line to bypass the code that uses the chooser to select the driver and replace it with code that accesses the driver directly via its ProgId.
//#define UseChooser

using System;
using ASCOM.TLTinylibHttpCam01;

namespace ASCOM
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if UseChooser
            // Choose the device
            string id = ASCOM.DriverAccess.Camera.Choose("");

            // Exit if no device was selected
            if (string.IsNullOrEmpty(id))
                return;

            // Create this device
            ASCOM.DriverAccess.Camera device = new ASCOM.DriverAccess.Camera(id);
#else
            // Create the driver class directly.
            //ASCOM.DriverAccess.Camera device = new ASCOM.DriverAccess.Camera("ASCOM.TLTinylibHttpCam01.Camera");
            ASCOM.TLTinylibHttpCam01.Camera.Camera device = new ASCOM.TLTinylibHttpCam01.Camera.Camera();
#endif

            // Connect to the device
            device.Connected = true;

            // Now exercise some calls that are common to all drivers.
            Console.WriteLine($"Name: {device.Name}");
            Console.WriteLine($"Description: {device.Description}");
            Console.WriteLine($"DriverInfo: {device.DriverInfo}");
            Console.WriteLine($"DriverVersion: {device.DriverVersion}");
            Console.WriteLine($"InterfaceVersion: {device.InterfaceVersion}");

            //
            // TODO add more code to test your driver.

            device.StartExposure(0.01, true);

            device.Connected = false;

            // Disconnect from the device
            device.Connected = false;

            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}
