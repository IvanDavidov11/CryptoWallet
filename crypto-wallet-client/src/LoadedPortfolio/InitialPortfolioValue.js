import React from 'react';
import { useState, useEffect } from 'react';

const InitialPortfolioValue = () => {
  const calculateInitial_ApiUrl = "https://localhost:7038/api/calc/initial";
  const [initialValue, setInitialValue] = useState(0);

  const formattedInitialValue = initialValue.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });


  useEffect(() => {
    const fetchInitalValue = async () => {
      try {
        const response = await fetch(calculateInitial_ApiUrl);

        if (!response.ok) throw Error('Did not receive expected data');

        const initialValueJson = await response.json();
        setInitialValue(parseFloat(initialValueJson));
      }
      catch (err) {
        console.log(err);
      }
    }
    fetchInitalValue();
  }, []);

  return (
    <div className='portfolio-infobox'>
      <p>Initial Value:</p>
      <p><strong>${formattedInitialValue}</strong></p>
    </div>
  )
}

export default InitialPortfolioValue