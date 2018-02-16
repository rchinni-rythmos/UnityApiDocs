Api Docs - C# Parser library - Requirements
--------------------------------------------

# Goals

- Fix GetTypeDocumentation() output xml
    [x] type attr in member node
    [x] remove extra member node in AClass
    [x] Use structure xml instead of plain signature
    [x] Test inner type documentation
    [x] Report explicit interface implementations

- Fix GetTypesXml()
    [x] Move inner types as top level xml nodes (include parentId node)
    [x] Add test for inner types

- [x] Add set of defines to be used

- Add tests for generics
    [ ] Methods
    [ ] Types

- [ ] Collect attribute



# Contents of the Repo
- The solution contains sample "Unity API" code. With sample code covering the scenarios listed in detailed requirements listed below
- And the output folder contains sample API responses, given the sample "Unity API" code

# APIs
1. Get All Types
    - Summary: Given a path to the folder, gets all the types that are part of the folder, and it's children.
    - Request: Path to the folder (relative)
    - Response: See sample output file  [GetAllTypes.xml](output/GetAllTypes.xml)
2. Get Type
    - Summary: Gets the documentation for a particular type.
    - Request: 
        - The full name of the type (Namespace + Name)
        - The relativeFilePaths where the code (.cs) files exist for the type. The output of "Get All Types" API contain these.
    - Response: sample outputs: [Object]("output/Object.xml"), [BillBoardAsset](output/BillboardAsset.xml), [PlayState](output/PlayState.xml)
        
3. Set Type
    - Summary: Saves the documentation for a particular type.
    - Request: 
        - The full name of the type (Namespace + Name)
        - The relativeFilePaths where the code (.cs) files exist for the type. The output of "Get All Types" API contain these.
    - Response:
        - None (for success)
        - Exception on failure

# Detailed Requirements for the APIs
1. System should output Xml Comments only for the Members where documentation needs to be generated (Public, Protected types) 
2. Include everything, that is part of the /// comments for a type, member
3. If no documentation is found for certain member/ type, it should not be excluded in the output. 
4. Handle Partial types (Both Get and set)
5. Provide the access modifier (public/ protected..). Needed if grouping by Public Members, Protected Members is needed
6. Indicate Static for members, types
7. Indicate Obsolete for members and types
8. Provide type of Type (Class, Interface, Enum…)
9. Provide type of Member (Method, Property, Constructor, Delegate etc.)
10. Undoc - to exclude documentation for certain members
11. For each Member, the value of "name" attribute is key. 
12. Include Method Signature. 
	a. For Param Data Type
	b. For Return Data Type
13. Include Parent type for a type
14. TBD: There is some documentation, that is not part of the code. Example: Messages section in https://docs.unity3d.com/ScriptReference/MonoBehaviour.html. These were specified as "docOnly=true" in the Mem.xml on the section xml tag. Need to discuss the right place for them to be stored in the code.

# What CMS would modify?
From the responses given by the C# parser, users in CMS would only be able to modify contents of the below tags:
1. Summary
2. Description
3. Example
4. Param
5. Returns
