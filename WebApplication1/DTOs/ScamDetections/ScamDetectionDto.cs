namespace ForumBE.DTOs.ScamDetections
{
    public class ScamDetectionDto
    {
        public class PredictionResponse
        {
            public PredictionData data { get; set; }
            public string message { get; set; }
            public int status { get; set; }
        }

        public class PredictionData
        {
            public float ConfidenceScore { get; set; }
            public DateTime CreatedAt { get; set; }
            public PredictionDetails Details { get; set; }
            public bool ModelPrediction { get; set; }
            public string ModelVersion { get; set; }
        }

        public class PredictionDetails
        {
            public List<string> Reasons { get; set; }
        }
    }
}
