using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Utilities
{
    public class StaticData
    {
        public const string DefaultFoodImage = "default_food.png";
        public const string DefaultImageExtension = ".png";
        public const string AdminEndUser = "Admin";
        public const string CustomerEndUser = "Customer";

        public const string StatusSubmitted = "Submitted";
        public const string StatusBeingPrepared = "Being prepared";
        public const string StatusReadyForPickup = "Ready for pickup";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
    }
}
