# UserAPI

API to Manage Users on Lorien. Used to create, update, read and delete Users. (Default Port: 5010)

## User Data Format

These are the fields of the user and it's constrains:

* userId: Id of the user given by de Database.
  * Integer
  * Ignored on Create, mandatory on the other methods
* username: username used for login.
  * String (Up to 50 chars)
  * Mandatory
* password: Hashed Password
  * String (Up to 250 chars)
  * Mandatory
* name: Name of the user
  * String (Up to 50 chars)
  * Mandatory
* email: E-mail of the user
  * String (Up to 50 chars)
  * Mandatory
* enabled: Indicates if the group is enabled or disabled
  * Boolean (Default = True)
  * Ignored on Create, mandatory on the other methods
* userGroup: Group of the user
  * Group JSON
  * Ignored on Create and Update
### JSON Example:
```json
{
    "userId": 1,
    "username": "6666",
    "password": "13464987",
    "email": "marcus.santos@integradora.com.br",
    "name":"Marcus",
    "enabled": false,
    "userGroup": null
}
```

## URLs

* api/users/{optional=startat}{optional=quantity}{optional=orderField}{optional=order}{optional=fieldFilter}{optional=fieldValue}

  * Get: Return List of Users
    * startat: represent where the list starts t the database (Default=0)
    * quantity: number of resuls in the query (Default=50)
    * orderField: Field in which the list will be order by (Possible Values:
      username,email)(Default=username)
    * order: Represent the order of the listing (Possible Values: ascending,
      descending)(Default=Ascending)
    * fieldFilter: represents the field that will be seached  (Possible Values:
      username,email)(Default=null)
    * fieldValue: represents de valued searched on the field (Default=null)
  * Post: Create the User with the JSON in the body
    * Body: User JSON

* api/users/{id}
  * Get: Return User with userid = ID
  * Put: Update the User with the JSON in the body with userid = ID
    * Body: User JSON
  * Delete: Disable User with userid = ID

* api/users/list{userid}{userid}
  * Get: Return List of Users with userid = ID

# PermissionAPI

API to Manage Permissions on Lorien. Used to create, update, read and delete Users.

## User Data Format
The permissions JSON is set o Key value pair. Containing the permission name and it´s description:
### JSON Example:
```json
{
    "user__allow_read": "UserAPI Allow Read",
    "user__allow_update": "UserAPI Allow Update"
}
```
## URLs
* api/permissions/
  * Get: Return List of Permissions available in the system

# UserGroupAPI

API to Manage User Groups on Lorien. Used to create, update, read and delete User Groups.

## User Group Data Format

These are the fields of the user and it's constrains:

* userGroupId: Id of the user group given by de Database.
  * Integer
  * Ignored on Create, mandatory on the other methods
* name: username used for login.
  * String (Up to 50 chars)
  * Mandatory
* description: Description of the Group
  * String (Up to 100 chars)
  * Mandatory
* users: List of Users in the group
  * List of Users JSON
  * Ignored on Create and Update
* permissions: List of the groups permissions
  * List of permissions
  * Ignored on Create and Update
* enabled: indicates if the group is enabled or disabled
  * Boolean (Default = True)
  * Ignored on Create, mandatory on the other methods
### JSON Example:
```json
{
    "userGroupId": 1,
    "name": "grupoteste",
    "description": "grupoteste",
    "users": [
        {
            "userId": 3,
            "username": "teste2",
            "name": "teste2",
            "password": "teste2",
            "email": "marcus.santos@integradora.com.br",
            "enabled": true
        }
    ],
    "enabled": true,
    "permissions": [
        "user__allow_read"
    ]
}
```

## URLs

* api/usergroupss/{optional=startat}{optional=quantity}{optional=orderField}{optional=order}{optional=fieldFilter}{optional=fieldValue}

  * Get: Return List of User Groups
    * startat: represent where the list starts at the database (Default=0)
    * quantity: number of resuls in the query (Default=50)
    * orderField: Field in which the list will be order by (Possible Values:
      name,description)(Default=name)
    * order: Represent the order of the listing (Possible Values: ascending,
      descending)(Default=Ascending)
    * fieldFilter: represents the field that will be seached  (Possible Values:
       name,description)(Default=null)
    * fieldValue: represents de valued searched on the field (Default=null)
  * Post: Create the User with the JSON in the body
    * Body: User Group JSON

* api/usergroupss/{id}
  * Get: Return User with usergroupid = ID
  * Put: Update the User with the JSON in the body with usergroupid = ID
    * Body: User Group JSON
  * Delete: Disable User with usergroupid = ID

* api/usergroups/list{usergroupid}{usergroupid}
  * Get: Return List of Users with usergroupid = ID

* api/usergroups/users/{id}
  * Post: Add the user to the group
    * Body: User JSON
  * Delete: Remove the user from the group User with usergroupid = ID
    * Body: User JSON

* api/usergroups/permissions/{id}permission={permission}
  * Post: Add the permission to the group
  * Delete: Remove the permission from the group User with usergroupid = ID

# LoginAPI

API to get login info.

## User Login Format

These are the fields of the user and it's constrains:

* username: username used for login.
  * String (Up to 50 chars)
  * Mandatory
* password: Hashed Password
  * String (Up to 250 chars)
  * Mandatory

### JSON Example:
```json
{
  "password": "2",
  "username": "teste1"
}
```
## URLs

* api/login/
  * Post: Get the user login info Hashed
    * Body: User Login JSON
    * Returns: User Secured information.


### JSON Example for Secured User Information:
```json
{
    "name": "teste2",
    "security": "CfDJ8D81hm4ZEFZJlBwEqRbZXj9XCdNtlEGuoRm4d42LbQxy-L_EDU61QwmmMF2N82UsdfJD4hexGKe8d3QECAN4DfjC5cXpXwnYEr2ymidc8jyG-9y8NThRJtHhGnC66AiJvG__SHLUEccL5YF2ZG2_682amxDwKgUj0RhloTPzIFmA2i4BYNPPftDd6a9_ULbe1Szor5EOuF2jAlD-WLVjFq1qCSHyQQ_8E2MFjK1V5LQx3wy7eeANxZCpq9Vkpaxvv7NoNQJC941AGJB8-WOEx8mjVQdtLc_x-24JvPPEvXbLFDyH5DThih5ibJptLXdNjlcjqOGpVlmCEyzGFO-qxHlmj20ZoFBg2piX7cvauRB2GcTrJuWSo5eLyI-X23ewj_PpTfFz4OL0ngPZrMd-7U6pMt2mQB6ZJQ7TNziYVPO8WR93nXdMXKSPWC4-yWUDXflXmU5MLqwyW4XrjURktow"
}
```