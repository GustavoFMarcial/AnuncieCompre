export function createDate(date: string | Date){
    if (date == "0001-01-01T00:00:00"){
        return null;
    }
    else{
        return dateMask(new Date(date));
    }
}

function dateMask(date : Date){
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
}