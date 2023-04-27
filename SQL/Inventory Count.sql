 SELECT bk.Id, bk.Brand, bk.Color,
                            own.Id as OwnerId, own.Name, own.Address, own.Email, own.Telephone,
                            bt.Name,
                            wo.Id as WorkOrderId, wo.Description, wo.DateInitiated, wo.DateCompleted
                            FROM Bike bk 
                                JOIN Owner own on bk.OwnerId = own.Id
                                JOIN BikeType bt on bk.BikeTypeId = bt.Id
                                JOIN WorkOrder wo on wo.BikeId = bk.Id

                                 SELECT bk.Id as BikeId, bk.Brand, bk.Color,
                            own.Id as OwnerId, own.Name, own.Address, own.Email, own.Telephone,
                            bt.Id as btId, bt.Name,
                            wo.Id as WorkOrderId, wo.Description, wo.DateInitiated, wo.DateCompleted
                            FROM Bike bk 
                                JOIN Owner own on bk.OwnerId = own.Id
                                JOIN BikeType bt on bk.BikeTypeId = bt.Id
                                JOIN WorkOrder wo on wo.BikeId = bk.Id
                            WHERE wo.DateCompleted is NULL
                             group by BikeId
                              
                              

                                   SELECT count(distinct  bk.Id) as BikeId 
                            
                            FROM Bike bk 
                                JOIN Owner own on bk.OwnerId = own.Id
                                JOIN BikeType bt on bk.BikeTypeId = bt.Id
                                JOIN WorkOrder wo on wo.BikeId = bk.Id
                            WHERE wo.DateCompleted is NULL
                            group by BikeId
                                                        order BY BikeId

                             