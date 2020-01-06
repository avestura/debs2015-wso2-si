# Query1 Spec

### Input:

- pickup_time
- dropoff_time
- pick_lat -> to cell id
- pick_long -> to cell id
- drop_lat -> to cell id
- drop_long -> to cll id


### Output:

- pickup_time
- dropoff_time
- start_cell [1..10]
- end_cell [1.10]
- delay


# Query2 Spec

profitability:

 ((median fare) + (tip for last 15 min))
div by (empty taxis for 30 min)