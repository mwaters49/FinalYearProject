# AI Tag Game

Final year project by **Matthew Waters**.

A Unity-based AI tag game built with **C#** and **Unity ML-Agents**, where an agent learns to play tag through reinforcement learning.

## Demo

- **Short demo video:** [Final Year Project Short Video](https://youtu.be/u5A_YbMftX8)
- **Full presentation video:** [Final Year Project Presentation](https://youtu.be/NtBhVstCTzo)

## Overview

This project explores how reinforcement learning can be applied to a simple game of tag inside Unity.

The agent was trained to move around an arena, pursue a target, and improve its behaviour over time using rewards and penalties. Positive reward was given for successful tags, while negative reward was used for undesirable behaviour such as colliding with walls or taking too long.

## What the project includes

- Unity gameplay prototype for an AI tag game
- C# scripts for player movement, AI movement, and tag detection
- ML-Agents training setup and trained model files
- TensorBoard result folders from training runs
- Final year report, presentation, and poster

## Key features

- **Reinforcement learning in Unity** using ML-Agents
- **Agent movement and decision making**
- **Tag-based game logic**
- **Reward shaping** for better training behaviour
- **Saved trained models** for reuse and evaluation
- **Supporting academic deliverables** for presentation and documentation

## Tech stack

- **Unity**
- **C#**
- **Unity ML-Agents**
- **TensorBoard**
- **ONNX** model exports

## Repository structure

```text
C# Scripts/                 Core gameplay and AI scripts
Results for TensorBoard/    Training output and TensorBoard result folders
Trained Brain Files/        Trained model files and config
deliverables/               Report, presentation, poster, and diagrams
equations.txt               Jump / movement calculation notes
projectJournal.txt          Project progress journal
