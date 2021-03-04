# Lifecycle of perform migration

1. Publish running event
2. Get scripts already executed
3. Collect script from providers
4. Remove scripts which have already been run
5. For each script which needs to be run
   1. Execute the script
   2. Add executed script to the journal
   3. Execute post-script action
6. Publish succeeded event
