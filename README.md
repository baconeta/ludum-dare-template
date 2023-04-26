# ludum-dare-template

A template project used primarily for ludum dare gamejams. 

Contains a few preset up functionality and systems, including:

- Basic generic achievement system (uses user prefs for data so is not cheat safe) - possible improvement, add cheat protection.
- Scalable and generic UI composite state switching system with template setups
- Editor tools including ReadOnlyAtrributes for vars in inspector, and struct editing in place for inspector references
- Audio system with mix groups, object pooling and easy volume integration
- A simple template main menu screen

## Tools and dependencies
- [Unity 2022.2.12f1](https://unity3d.com/unity/whats-new/2022.2.12)
- [git LFS](https://git-lfs.github.com/) _Note, not used in the template as git does not support lfs templating, so needs to be manually enabled_

## Links
- Game Jam entry: XXX
- Itch link to play: XXX

## Development team

## Deploying Infrastructure
Sign up for [Google Cloud](https://cloud.google.com/free)

As per https://firebase.google.com/docs/projects/terraform/get-started:
1. Install [Terraform](https://learn.hashicorp.com/tutorials/terraform/install-cli?in=terraform/gcp-get-started)
2. Install [Google Cloud CLI](https://cloud.google.com/sdk/docs/install-sdk)
3. Login with `gcloud auth application-default login`
4. Edit [Infrastructure/vars.tf](Infrastructure/vars.tf) and set the values you need
    - It's important to set `unrelated_gcp_project_to_verify_billing`. When you signed up for Google Cloud it should have created a project, you can use that ID.
5. Run `terraform -chdir=Infrastructure init`
6. Run `terraform -chdir=Infrastructure apply -auto-approve`
7. View the infrastructure at https://console.firebase.google.com/ or https://console.cloud.google.com/welcome

### Configuring Firebase with Unity
As per https://firebase.google.com/docs/unity/setup#add-config-file

1. Open the [Firebase console](https://console.firebase.google.com/project/) and select the recently created project
2. In the left menu, next to Project Overview click the Gear and select Project Settings.
3. On the General tab, scroll down to "Your apps"
4. Click the Android app and download the `google-services.json` file.
5. Click the iOS app and download the `GoogleService-info.plist` file.
6. Copy these files into the [Assets](./Assets/) folder
7. Go to https://developers.google.com/unity/packages#firebase and download the following packages:
    - [Cloud Firestore](https://developers.google.com/unity/packages#cloud_firestore)
    - [Firebase Authentication](https://developers.google.com/unity/packages#firebase_authentication)
8. In your open Unity project, navigate to Assets | Import Package | Custom Package.
9. In the Import Unity Package window, click Import.

### Troubleshooting
If you encounter a 403 saying that the Firebase Management API or another permission has not been enabled, wait a little while and run `terraform apply` again.