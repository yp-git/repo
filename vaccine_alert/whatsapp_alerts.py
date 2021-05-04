import pywhatkit as kit
from datetime import datetime, timedelta


class WhatsAppAlert:

    def send(msg,phone):
        time_change = datetime.now() + timedelta(minutes=1)
        hour = time_change.hour
        mint = time_change.minute
        #kit.check_window()
        kit.sendwhatmsg(phone_no=phone,message=msg,time_hour=hour,time_min=mint)


    def send_to_group(msg,group):
        time_change = datetime.now() + timedelta(minutes=1)
        hour = time_change.hour
        mint = time_change.minute
        #kit.check_window()
        kit.sendwhatmsg_to_group(group_id=group,message=msg,time_hour=hour,time_min=mint)    