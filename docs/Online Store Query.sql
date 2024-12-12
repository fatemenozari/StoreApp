SELECT 
    p.Id,
    p.Name,
    p.Description,
    p.Price,
    p.Stock,
    p.CategoryId,
    c.Discount
FROM 
    Products p
JOIN 
    Categories c ON p.CategoryId = c.Id
ORDER BY

    CASE WHEN c.Discount = 0 THEN 0 ELSE 1 END,

    c.Discount,

    CASE WHEN p.Stock = 0 THEN 1 ELSE 0 END;