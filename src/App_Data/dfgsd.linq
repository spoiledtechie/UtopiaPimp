<Query Kind="Expression">
  <Connection>
    <ID>53c9abbb-8775-46cf-b2f5-abd3ab291b22</ID>
    <Server>.\SQLEXPRESS</Server>
    <AttachFile>true</AttachFile>
    <UserInstance>true</UserInstance>
    <AttachFileName>G:\UP\App_Data\UP.mdf</AttachFileName>
    <Persist>true</Persist>
  </Connection>
</Query>

from xx in Utopia_Province_Data_Captured_Gens
from yy in Utopia_Province_Identifiers
where xx.Province_ID == yy.Province_ID
where xx.Monarch ==1
select new {xx.Province_ID, yy.Province_Name, xx.Monarch}