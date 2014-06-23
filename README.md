doyourtestsenablechange
=======================

'''PAYLESS
Payless will take payments from account holders in the UK and Australia.

UK account numbers have 16 digits and start with 44.
AU account numbers have 10 digits and end with 19.

A payment must be marked as micro is it is below a certain monetry value.
In the UK this is amounts less than £5.
In AU this is amounts less than $6.09


To make a payment clients must POST to this URL:
http://....payless/makepurchase

And include the correct parameters on a query string.


eg.

http://....payless/makepurchase?accountnumber=44234567890&location=UK&amount=1.99&currency=GBP&type=micro

Parameter details and rules.

location: must be UK (UK) AU (australia)
currency: GBP (£) AUD($)
type: micro (or left out completely)

Account numbers are validated according to the rules above.
Payments below the described amount should be marked as 'micro' in the type field.

The request will return a plain text response containing the id of the purchase.


'''PAYLESS refund

http://...payless/refundpurchase?accountnumber=44234567890&location=UK&purchaseId={GUID}

Parameter details
location, accountnumber as above
purchaseId supplied must be match those stored by PAYLESS for the supplied PurchaseId

Response will be a success/failure.

ONLY FULL REFUNDS ARE PERMITTED. requests containing amounts will be REJECTED.

'''PAYMORE
As above in terms of parameters supplied (and their validation)
BUT
type must be ommitted
amounts must be over (or equal to) £5.00 or $6.09 other payments must be made through PAYLESS
AND
parameters in form POST body
response is 302 with the purchase resource (JSON)
HEADER: 302
BODY: {link:/paymore/.../Guid}

Refund - DELETE this resource.


















