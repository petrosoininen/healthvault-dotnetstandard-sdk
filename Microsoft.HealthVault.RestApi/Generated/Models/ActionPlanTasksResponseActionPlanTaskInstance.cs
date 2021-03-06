// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.HealthVault.RestApi.Generated.Models
{
    using Microsoft.HealthVault;
    using Microsoft.HealthVault.RestApi;
    using Microsoft.HealthVault.RestApi.Generated;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The action plans task collection response
    /// </summary>
    public partial class ActionPlanTasksResponseActionPlanTaskInstance
    {
        /// <summary>
        /// Initializes a new instance of the
        /// ActionPlanTasksResponseActionPlanTaskInstance class.
        /// </summary>
        public ActionPlanTasksResponseActionPlanTaskInstance()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// ActionPlanTasksResponseActionPlanTaskInstance class.
        /// </summary>
        /// <param name="tasks">The collection of tasks</param>
        /// <param name="nextLink">The URI for the next page of data</param>
        public ActionPlanTasksResponseActionPlanTaskInstance(IList<ActionPlanTaskInstance> tasks = default(IList<ActionPlanTaskInstance>), string nextLink = default(string))
        {
            Tasks = tasks;
            NextLink = nextLink;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the collection of tasks
        /// </summary>
        [JsonProperty(PropertyName = "tasks")]
        public IList<ActionPlanTaskInstance> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the URI for the next page of data
        /// </summary>
        [JsonProperty(PropertyName = "nextLink")]
        public string NextLink { get; set; }

    }
}
