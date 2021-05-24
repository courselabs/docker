# Lab Hints

Image tags are often used for application versions, and a [semantic versioning]() approach is very common. 

You might have multiple tags for the same image:

- 1 - major version
- 1.0 - major + minor version
- 1.0.100 -  major + minor version + build number

That lets users choose to pin to a specific version - `1.0.100` will never change; or to a minor version - `1.0` is `1.0.100` now but could be an alias for `1.0.126` next month; or to a major version - `1` is `1.0.100` now, but it could be `1.2.407` next year.

The RNG app uses a similar approach.

> Need more? Here's the [solution](solution.md).