scan.data <- read.csv("data.csv")
products <- read.csv("Products1.txt", sep = '|')
join.data <- merge(x=scan.data, y=products, by.x="ProductPurchased", by.y="SKU")
rm(scan.data, products)
join.data <- join.data[-c(5,7,9)]


nrow(join.data[which(join.data$ItemType == "Milk"),])/365

nrow(join.data[which(join.data$ItemType == "Cereal"),])/365

nrow(join.data[which(join.data$ItemType == "Baby Food"),])/365

nrow(join.data[which(join.data$ItemType == "Diapers"),])/365

nrow(join.data[which(join.data$ItemType == "Bread"),])/365

nrow(join.data[which(join.data$ItemType == "Peanut Butter"),])/365

nrow(join.data[which(join.data$ItemType == "Jelly/Jam"),])/365

nrow(join.data[which(
    join.data$ItemType != "Milk" &
    join.data$ItemType != "Cereal" &
    join.data$ItemType != "Baby Food" &
    join.data$ItemType != "Diapers" &
    join.data$ItemType != "Bread" &
    join.data$ItemType != "Peanut Butter" &
    join.data$ItemType != "Jelly/Jam"),])/365
