# Kubernetes Job Example with C# Client

This project demonstrates how to dynamically create and monitor Kubernetes Jobs using the Kubernetes C# Client. The application connects to a Kubernetes cluster, submits a Job, and continuously polls its status until completion.

## Features

- Create Kubernetes Jobs dynamically from a C# console application.
- Monitor the job's status in real-time.
- Simple configuration to work with local or remote clusters via kubeconfig.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- A running Kubernetes cluster (local or remote).
- A valid kubeconfig file (typically located at `~/.kube/config`).
- Kubernetes C# Client NuGet package installed (automatically included in the project).

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/KubernetesJobExample.git
   cd KubernetesJobExample
