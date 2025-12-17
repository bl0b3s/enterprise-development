using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain_.Models;


/// <summary>
/// Перечисление типов привода
/// </summary>
public enum DriveType
{
    /// <summary>
    /// Передний привод (FWD)
    /// </summary>
    FrontWheelDrive,

    /// <summary>
    /// Задний привод (RWD)
    /// </summary>
    RearWheelDrive,

    /// <summary>
    /// Полный привод (AWD)
    /// </summary>
    AllWheelDrive
}
