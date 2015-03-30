
namespace ErrorHandlerEngine.Shared
{
    public class OperationSystemInfo
    {
        public string Name { get; set; }
        public string Edition { get; set; }
        public string ServicePack { get; set; }
        public string Version { get; set; }
        public string ProcessorBits { get; set; }
        public string OsBits { get; set; }
        public string ProgramBits { get; set; }

        public OperationSystemInfo(bool currentSystem = false)
        {
            if (currentSystem)
            {
                Name = OperationSystem.Name;
                Edition = OperationSystem.Edition;
                ServicePack = OperationSystem.ServicePack;
                Version = OperationSystem.VersionString;
                ProcessorBits = OperationSystem.ProcessorBits.ToString();
                OsBits = OperationSystem.OsBits.ToString();
                ProgramBits = OperationSystem.ProgramBits.ToString();
            }
        }

        public override string ToString()
        {
            var sp = ServicePack != "";
            return string.Format("{0}{1}{2} {3}Bit v{4} - {5}Bit Processor Architect - {6}Bit Application",
                Name,
                sp ? "SP" : "",
                sp ? ServicePack : "",
                OsBits.Replace("Bit", ""),
                Version,
                ProcessorBits.Replace("Bit", ""),
                ProgramBits.Replace("Bit", ""));
        }
    }
}
