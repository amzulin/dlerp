using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Enterprise.Invoicing.Web
{
    //This class complement the softdog function,and import the dll interface 
    public unsafe class Dog
    {

        public uint DogBytes, DogAddr;
        public byte[] DogData;
        public uint Retcode;

        //发布到64位机器上dll要改为64dll.dll
        //vs程序开发则用32.dll
        [DllImport("win64dll.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint DogRead(uint idogBytes, uint idogAddr, byte* pdogData);
        [DllImport("win64dll.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint DogWrite(uint idogBytes, uint idogAddr, byte* pdogData);

        public unsafe Dog(ushort num)
        {
            DogBytes = num;
            DogData = new byte[DogBytes];
        }

        public unsafe void ReadDog()
        {
            fixed (byte* pDogData = &DogData[0])
            {
                Retcode = DogRead(DogBytes, DogAddr, pDogData);
            }
        }
        public unsafe void WriteDog()
        {
            fixed (byte* pDogData = &DogData[0])
            {
                Retcode = DogWrite(DogBytes, DogAddr, pDogData);
            }
        }
    }
}
