import { useEffect, useState } from 'react';
import Coin from './Coin';

function App() {
  const API_Url = "https://localhost:7038/api/coins"
  const [coins, setCoins] = useState([]);
  const [isLoading, setIsLoading] = useState(true);


  useEffect(() => {

    const fetchItems = async () => {
      try {
        const response = await fetch(API_Url);

        if (!response.ok) throw Error('Did not receive expected data');

        const listCoins = await response.json();
        console.log(listCoins);
        setCoins(listCoins);
      }
      catch (err) {
        console.log(err);
      }
      finally {
        setIsLoading(false);
      }
    }
    fetchItems();
  }, [])

  return (
    <div className="App">
      {isLoading ? (
        <p>Currently loading</p>) : (
        <ul>
          {coins.map((coin) =>
            <Coin
              key={coin.id}
              coin={coin} />
          )}
        </ul>
      )}
    </div>
  );
}

export default App;
