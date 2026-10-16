# Unity CLI terminal automation and Pipeline editor control

Unity CLI is an experimental standalone command-line tool for installing Editors, adding modules and managing projects. Controlling a running Unity Editor is a separate layer that requires the Unity Pipeline package.

Unity CLI covers terminal-first Editor and project management, CI workers, structured output and predictable exit codes. Pipeline adds the separate capability to send commands to a running Editor through its local HTTP API.

## Current compatibility and prerequisites

Unity's official documentation inspected on 2026-10-11 lists these Unity CLI operating systems:

- Windows 10 21H1 or newer
- RHEL 9 or Ubuntu 22.04 or later with glibc 2.34 or newer
- macOS 14 or newer

The same page says Debian 11, CentOS 8 and Amazon Linux 2 are not supported. It also says the CLI can run on headless CI workers without the Unity Hub desktop application.

The Pipeline package requires Unity Editor 6.0 or later and Unity CLI. Unity describes `com.unity.pipeline` as the package that controls the Editor locally for automation, testing and CI/CD integration.

## Documented Editor-control sequence

After checking the current prerequisites and installation instructions:

1. Open the project in Unity Editor 6.0 or later.
2. Sign in with `unity auth login`.
3. Install the project package and its dependencies with `unity pipeline install`.
4. Return to the Editor and wait for the project to recompile.
5. Verify the connection with `unity pipeline list`.
6. Run `unity command` from the project directory to list available Editor commands.
7. If several Editor instances are open, select one with `unity command --project-path=<path-to-your-project>`.

These are documented commands, not commands executed for this article.

## Evidence and limits

This page records official Unity documentation inspected on 2026-10-11. No CLI installation, authentication, package installation, Editor connection, scene mutation, compilation, test run, capture, build or CI worker was exercised. Because Unity labels the CLI experimental, command behavior and prerequisites can change.

The inspected pages do not directly establish pricing, subscription separation or license eligibility, so this article makes no claim about them. Check current Unity licensing and service terms separately before adoption.

Recheck the official pages when the CLI, Pipeline package, operating system or Editor version changes, and before relying on older MCP or Unity AI setup instructions.

## Official sources

- [Use the Unity command-line interface](https://docs.unity.com/en-us/unity-cli/use-unity-cli): experimental status, terminal and CI uses, operating-system support and the Pipeline requirement for Editor control.
- [Unity Pipeline package](https://docs.unity.com/en-us/unity-cli/unity-pipeline/unity-pipeline-package): Editor and CLI prerequisites, package installation, connection checks and `unity command` routing.
- [Local tools and Unity CLI](https://docs.unity.com/en-us/unity-production-pipeline/local-tools-cli): the relationship between Unity CLI, `com.unity.pipeline`, local automation and external systems.
