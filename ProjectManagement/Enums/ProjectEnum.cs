using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Enums
{
    public enum EField
    {
        [Display(Name = "SoftwareDevelopment")]
        SoftwareDevelopment,
        [Display(Name = "Cybersecurity")]
        Cybersecurity,
        [Display(Name = "DataScienceAndAnalytics")]
        DataScienceAndAnalytics,
        [Display(Name = "ArtificialIntelligenceAndMachineLearning")]
        ArtificialIntelligenceAndMachineLearning,
        [Display(Name = "CloudComputing")]
        CloudComputing,
        [Display(Name = "InternetOfThings")]
        InternetOfThings,
        [Display(Name = "MobileDevelopment")]
        MobileDevelopment,
        [Display(Name = "WebDevelopment")]
        WebDevelopment,
        [Display(Name = "ComputerNetworking")]
        ComputerNetworking,
        [Display(Name = "VirtualRealityAndAugmentedReality")]
        VirtualRealityAndAugmentedReality,
        [Display(Name = "BigData")]
        BigData,
        [Display(Name = "BlockchainTechnology")]
        BlockchainTechnology,
        [Display(Name = "HumanComputerInteraction")]
        HumanComputerInteraction,
        [Display(Name = "QuantumComputing")]
        QuantumComputing,
        [Display(Name = "Bioinformatics")]
        Bioinformatics
    }

    public enum ELevel
    {
        [Display(Name = "Easy")]
        Easy,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "Difficult")]
        Difficult
    }

    public enum EProjectStatus
    {
        [Display(Name = "Waiting")]
        WAITING,
        [Display(Name = "Published")]
        PUBLISHED,
        [Display(Name = "Registered")]
        REGISTERED,
        [Display(Name = "Processing")]
        PROCESSING,
        [Display(Name = "Completed")]
        COMPLETED,
        [Display(Name = "GaveUp")]
        GAVEUP
    }
}
