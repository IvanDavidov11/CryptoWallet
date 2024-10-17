import { useEffect, useState } from 'react';
import EmptyPortfolio from './EmptyPortfolio/EmptyPortfolio';
import LoadedPortfolio from './LoadedPortfolio/LoadedPortfolio';
import SpinnerLoader from './SpinnerLoader';

function App() {
  const main_ApiUrl = "https://localhost:7038/api/coins";
  const hasCoins_ApiUrl = "https://localhost:7038/api/coins/has-coins";
  const [hasCoins, setHasCoins] = useState(false);
  const [coins, setCoins] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [fileUploaded, setFileUploaded] = useState(false);

  useEffect(() => {
    if (hasCoins) {
      fetchCoins();
    }
  }, [hasCoins]);

  useEffect(() => {
    checkIfHasCoins();
  }, [fileUploaded]);

  const handleFileUpload = () => {
    setFileUploaded(true);
  };

  const fetchCoins = async () => {
    try {
      setIsLoading(true);
      const response = await fetch(main_ApiUrl);

      if (!response.ok) throw Error('Did not receive expected data');

      const listCoins = await response.json();
      console.log(`FetchCoins executed: ${listCoins}`);
      setCoins(listCoins);
    }
    catch (err) {
      console.log(err);
    }
    finally {
      setIsLoading(false);
    }
  }

  const checkIfHasCoins = async () => {
    try {
      setIsLoading(true);
      const response = await fetch(hasCoins_ApiUrl);

      if (!response.ok) throw Error('Did not receive expected data');

      const hasCoinsValue = await response.json();
      console.log(`HasCoins executed: ${hasCoinsValue}`);
      setHasCoins(hasCoinsValue);
    }
    catch (err) {
      console.log(err);
    }
    finally {
      setIsLoading(false);
    }
  }

  return (
    <div className='app'>
      <div className='centered-content'>

        {!hasCoins ? (
          <EmptyPortfolio
            onFileUpload={handleFileUpload}
            setIsLoading={setIsLoading}
            isLoading={isLoading}
          />
        ) : (
          <LoadedPortfolio
            coins={coins}
            setFileUploaded={setFileUploaded}
            fetchCoins={fetchCoins}
            checkIfHasCoins={checkIfHasCoins} />
        )}
      </div>
    </div>
  );
}

export default App;