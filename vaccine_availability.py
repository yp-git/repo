from whatsapp_alerts import WhatsAppAlert
import requests
import time
from datetime import datetime, timedelta
import logging
logging.basicConfig(filename='vaccine.log', encoding='utf-8', level=logging.INFO)

def check_setu_availability(district,phones,groups):
    start_date = datetime.now()
    end_date =  datetime.now()+ timedelta(days=60)

    days_between = [(start_date + timedelta(days=x)).strftime('%d-%m-%Y') for x in range((end_date - start_date).days)]
    messages = []

    for v_date in days_between:     
        resp = requests.get('https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByDistrict?district_id={}&date={}'.format(district,v_date))
        
        if resp.status_code != 200:        
            WhatsAppAlert.send('+91xxxxxxxxx','Vaccine alert failed.')
        else:            
            sessions = resp.json()
            for session in sessions['sessions']:
                if ( session['name'] is not None ) and ('4' in str(session['pincode'])):
                    msg = 'DATE: {} name : {} avail :{} pincode : {} age_limit : {} vaccine : {}'.format(v_date,session['name'],session['available_capacity'],session['pincode'],session['min_age_limit'],session['vaccine'])                                  
                    messages.append(msg)    
            
            
    if len(messages)>0 : 
        final_msg = 'Vaccines available : {}'.format("\n".join(messages))    
 
        for phone_no in phones:            
            WhatsAppAlert.send(final_msg,phone_no)
            time.sleep(30)

        #for group in groups:            
            #WhatsAppAlert.send_to_group(final_msg,group)
            #time.sleep(30)
                

if __name__ == "__main__":
    logging.info('checked the status at {}'.format(datetime.now()))   

    #district wise
    #mumbai 395
    phones = ['+91XXXX','+91XXXXXX']         
    groups = ['Iuh9TsLn9ScDxxxxxxxx']
    check_setu_availability('395',phones,groups)
    
    #thane 394
    phones = ['+919XXXXXXXX','+91XXXXXXX']
    check_setu_availability('394',phones,groups)

    

    