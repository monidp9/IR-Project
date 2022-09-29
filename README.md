# Intelligent Robotics Project
## Academic project for the class "Intelligent Robotics"
The aim was to build a system in which an agent can autonomously drive without colliding with objects like walls or other agents. The repository contains only the main Assets for the Unity project. Thus, in order to let all the things work you should:
- Download [Unity](https://unity.com)
- Install [MLAgents](https://github.com/Unity-Technologies/ml-agents)
- Install [Racing Kit](https://www.kenney.nl/assets/racing-kit) as imported asset
- Download the Assets directory from this repository and substitute the Assets directory of the empty project created in Unity

The main scripts are the following:
- `CarDriver.cs`: contains all the logic for the agent movement and physical update.
- `CarDriverAgent.cs`: contains all the implementation of the Reinforment Learning life cycle; thus what to do at the beginning of the episod, on collect observations, on action received etc.
- `TrackCheckpoints.cs`: the logic for tracking the correct path of each car in the circuit.
- `CheckpointSingle.cs`: basic logic for each checkpoint.
