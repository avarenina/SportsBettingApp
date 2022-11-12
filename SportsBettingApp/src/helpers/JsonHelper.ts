export class JsonHelper
{
    public static Deserialize(data: string): any
    {
        return JSON.parse(data, JsonHelper.ReviveDateTime);
    }

    private static ReviveDateTime(key: any, value: any): any 
    {
        if (typeof value === 'string')
        {
            const a = /\/Date\((\d*)\)\//.exec(value);
            if (a)
            {
                return new Date(+a[1]);
            }
        }

        return value;
    }
}