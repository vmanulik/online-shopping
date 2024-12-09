#keycloak admin is available running on docker via url

localhost:9090

#client created in keycloak

online-shopping

#client scopes created in keycloak

cart, catalog

#roles created in keycloak

manager, customer

#policies created on top of the roles in keycloak

manager-policy, customer-policy

#client scope-level permissions created in keycloak

cart:read - associated with manager-policy, customer-policy on cart scope

cart:write - associated with manager-policy, customer-policy on cart scope

catalog:read - associated with manager-policy, customer-policy on catalog scope

catalog:write - associated with manager-policy on catalog scope

#miscellaneous

git pre-push validation using dotnet-format tool to check for consistent styling and rules
implemented using local pre-commit hook being copied to .git/hooks via post-build event