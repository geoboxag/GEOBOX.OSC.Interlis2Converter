namespace GEOBOX.OSC.Interlis2Converter.Common.Controllers
{
    /// <summary>
    /// All Controllers for Help and Execution
    /// </summary>
    public class AvailableControllers
    {
        /// <summary>
        /// Register for all Controllers
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string /*Key: FunctionType*/, Type /*Value: Controller-Class-Type*/> Get()
        {
            var availableControllers = new Dictionary<string, Type>
            {
                { MergeDMAV11.CommandType, typeof(MergeDMAV11) },
                { MergeDMAVZH11.CommandType, typeof(MergeDMAVZH11) },
                { ServiceDownload.CommandType, typeof(ServiceDownload) },
            };

            return availableControllers;
        }
    }
}
