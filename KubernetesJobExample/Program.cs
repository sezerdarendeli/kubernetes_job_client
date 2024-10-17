using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KubernetesJobExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Step 1: Set up the Kubernetes Client
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(); // or use BuildConfigFromEnv()
            var client = new Kubernetes(config);

            // Step 2: Define the dynamic job
            var job = new V1Job
            {
                ApiVersion = "batch/v1",
                Kind = "Job",
                Metadata = new V1ObjectMeta
                {
                    Name = "dynamic-job-" + Guid.NewGuid().ToString(),
                    NamespaceProperty = "default"
                },
                Spec = new V1JobSpec
                {
                    Template = new V1PodTemplateSpec
                    {
                        Metadata = new V1ObjectMeta
                        {
                            Labels = new Dictionary<string, string> { { "job-name", "dynamic-job" } }
                        },
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new V1Container
                                {
                                    Name = "my-container",
                                    Image = "busybox",
                                    Command = new List<string> { "sh", "-c", "echo Hello Kubernetes!" }
                                }
                            },
                            RestartPolicy = "Never"
                        }
                    },
                    BackoffLimit = 4 // Retry limit if the job fails
                }
            };

            // Step 3: Create the job in the Kubernetes cluster
            var jobResponse = client.CreateNamespacedJob(job, "default");
            Console.WriteLine($"Job created with name: {jobResponse.Metadata.Name}");

            // Step 4: Poll the job status to monitor completion
            var jobName = jobResponse.Metadata.Name;

            V1Job jobStatus = null;
            while (true)
            {
                jobStatus = client.ReadNamespacedJobStatus(jobName, "default");

                if (jobStatus.Status.Succeeded.HasValue && jobStatus.Status.Succeeded.Value > 0)
                {
                    Console.WriteLine($"Job {jobName} has completed successfully.");
                    break;
                }
                else if (jobStatus.Status.Failed.HasValue && jobStatus.Status.Failed.Value > 0)
                {
                    Console.WriteLine($"Job {jobName} has failed.");
                    break;
                }
                else
                {
                    Console.WriteLine($"Job {jobName} is still running...");
                }

                // Wait for a few seconds before checking the status again
                await Task.Delay(5000);
            }

            Console.WriteLine("Job monitoring complete.");
        }
    }
}
