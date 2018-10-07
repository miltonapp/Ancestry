# Ancestry

Sample API calls:
-------------------------------------------------------------------
Get all people whose name contains Dalenna:
    ../api/v1/person/Dalenna
    ../api/v1/person/Dalenna/Gender/MF

Get all female whose name contains Dalenna:
    ../api/v1/person/Dalenna/Gender/F

Get top 10 male whose name contains Dalenna:
    ../api/v1/person/Dalenna/Gender/M/10


-------------------------------------------------------------------
Get all Ancestors of Dalenna Shellie:
    ../api/v1/person/Dalenna Shellie/Ancestors
    ../api/v1/person/Dalenna Shellie/Ancestors/Gender/MF

Get all female Ancestors of Dalenna Shellie:
    ../api/v1/person/Dalenna Shellie/Ancestors/Gender/F

Get top 10 male Ancestors of Dalenna Shellie:
    ../api/v1/person/Dalenna Shellie/Ancestors/Gender/M/10


-------------------------------------------------------------------
Get all Descendants of Dalenna Shellie:
    ../api/v1/person/Dalenna Shellie/Descendants
    ../api/v1/person/Dalenna Shellie/Descendants/Gender/MF

Get all female Descendants of Dalenna Shellie:
    ../api/v1/person/Dalenna Shellie/Descendants/Gender/F

Get top 10 male Descendants of Dalenna Shellie:
    ../api/v1/person/Dalenna Shellie/Descendants/Gender/M/10

Unit testing TBD:
    Test each api with positive, nagative, boundaries and performance.
    Test invalid json format.
    Test invalid person data and invalid place data E.g. place id not found, person parents id reference missing/dead-looping.
    Test single parent.
    Test Ancestors with count included the right Ancestors(with geneder) cross multiple generations.

