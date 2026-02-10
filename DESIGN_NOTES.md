# TerminalCity Design Notes

Design philosophy, historical context, and gameplay concepts for TerminalCity.

---

## Economic Disruptions and City Evolution

### Core Insight: Disruption is Natural

Cities don't evolve linearly - they experience waves of disruption that fundamentally reshape commercial and residential patterns. Understanding these disruption cycles is key to realistic city simulation.

### Historical Disruption Examples

**Retail Evolution:**
- Small grocery stores → Supermarkets (business model disruption)
- Mom & pop shops → E-commerce (internet disruption)
- Malls everywhere → Big box stores + strip malls + online shopping
  - Question: Why are malls dying but towns still have 3+ areas with 5+ big box stores and strip malls of varying success?
  - Different value propositions? Convenience vs. experience vs. price?
  - Strip malls more adaptable to tenant changes?

**Entertainment:**
- Movie theaters → Cable TV → Streaming services → Mobile viewing
- COVID accelerated the shift away from theaters
- Each wave didn't eliminate the previous model, but fundamentally changed the economics

**Residential:**
- Urban density → Suburban sprawl (post-WWII)
- Bedroom communities as a disruption model (the game's starting scenario)
- Recent: Return to urban density in some markets

### The Bedroom Community Scenario

The game's starting scenario ("Suburban Spillover, 1955") is itself a disruption:
- Farmland → Suburban housing
- Metropolis workers living 15 miles out
- New infrastructure needs (roads, schools, utilities)
- Economic model: Residential tax base without providing jobs

### 1950s Town Financing

Historical context on how towns like Livonia were actually financed in the 1950s:
- [To be expanded with research findings]
- Post-WWII housing boom
- Federal highway funding
- GI Bill and home loans
- Municipal bonds and development deals

### Gameplay Implications

**Design Questions:**
1. How do we simulate the natural lifecycle of commercial areas?
2. Should the game allow malls to be built, then watch them decline?
3. How do we model the shift from one retail paradigm to another?
4. What causes some strip malls to thrive while others fail?
5. How do zoning decisions early in the game affect long-term economic viability?

**Potential Mechanics:**
- Building types have "eras" - optimal period, then decline
- Economic forces that favor certain building types over time
- External disruptions (internet era, big box stores) that player must adapt to
- Adaptive reuse of buildings (dead mall → mixed-use development)
- Player can resist trends (preserve downtown) or embrace them (zone for big box)

### Why City Builders Get Stale

**The Genre Problem: Not Enough Change**

Most city builders are growth simulators. You build, expand, optimize. Once you've "solved" the optimal layout, the challenge disappears. Real cities aren't static - they constantly adapt to:
- Economic disruptions (new retail models, industry shifts)
- Technology changes (cars, internet, remote work)
- Population shifts (suburbanization, re-urbanization)
- External shocks (recessions, pandemics, natural disasters)

**What's Missing:**
- Buildings that become obsolete over time
- Economic forces that make your "perfect" downtown less relevant
- Need to adapt infrastructure to changing patterns
- Decisions about what to preserve vs. what to let go

**The Challenge Should Be Adaptation, Not Just Growth:**
- Can you manage the transition from main street shops to strip malls?
- Do you fight against big box stores or zone for them?
- When the factory closes, does the neighborhood decline or transform?
- How do you handle a mall that's no longer economically viable?

### Design Philosophy

**Organic Change Over Bulldozing:**
- Cities should evolve, not just be built once and frozen
- Buildings should transition uses (farmhouse → residential when farm is sold)
- Economic pressures should naturally reshape the city
- Player manages transitions, not just initial placement
- The game stays interesting because you're constantly adapting to new realities

**Historical Authenticity:**
- 1955 start: Post-war boom, highway era, suburban expansion
- Technology and economic changes should feel period-appropriate
- Learn from real history: Livonia, Levittown, other planned communities

---

## Technology as Disruption: A Different Kind of Tech Tree

### Core Concept

Traditional tech trees are additive - each discovery unlocks new buildings/units/capabilities. But real technology is *disruptive* - it creates winners and losers, makes old jobs obsolete, and forces adaptation.

**Design Goal:** Tech advances should create interesting *problems* to solve, not just new toys to play with.

### Chronological Tech Tree (Additions + Disruptions)

#### Prehistoric Era

**Fire / Controlled Burning (~400,000 BCE)**
- **Adds:** Cooking, warmth, protection, tool hardening, land clearing
- **Disrupts:** Raw food gathering less viable, forests cleared permanently
- **New Problems:** Fire safety, deforestation, smoke pollution
- **Gameplay:** Enables permanent settlements, but creates first environmental impact

**Agriculture (~10,000 BCE)**
- **Adds:** Predictable food supply, population growth, permanent settlements, food storage
- **Disrupts:** Hunter-gatherer lifestyle obsolete, nomadic cultures displaced
- **New Problems:** Land ownership conflicts, class stratification (farmers vs. non-farmers), famine risk from crop failure
- **Gameplay:** Population boom, but increased vulnerability to drought/plague

**Animal Domestication (~10,000 BCE)**
- **Adds:** Transportation, labor power, food diversity (milk, eggs), warfare capability
- **Disrupts:** Wild animal populations, natural predator balance
- **New Problems:** Animal disease transmission to humans, grazing land conflicts
- **Gameplay:** Faster movement/trade, but zoonotic disease risk

**Wheel (~3500 BCE)**
- **Adds:** Efficient transport, pottery wheel (better storage), military chariots
- **Disrupts:** Human/animal carrying less competitive
- **New Problems:** Road maintenance needs, wheel-wright profession vs. basket-makers
- **Gameplay:** Trade routes viable, but infrastructure demands increase

**Writing (~3200 BCE)**
- **Adds:** Record keeping, laws, history, contracts, long-distance communication
- **Disrupts:** Oral tradition/memory specialists less valuable, knowledge centralized
- **New Problems:** Literacy divide creates power imbalance, bureaucracy emerges
- **Gameplay:** Complex governance possible, but literate elite vs. illiterate masses

**Bronze Working (~3000 BCE)**
- **Adds:** Better tools, weapons, construction, trade goods
- **Disrupts:** Stone tool makers obsolete, copper-only regions disadvantaged
- **New Problems:** Resource wars (tin is rare), military imbalance
- **Gameplay:** Military/construction advantage, but requires rare resource trade networks

#### Classical Era

**Iron Working (~1200 BCE)**
- **Adds:** Cheaper, more available tools/weapons than bronze, better plows
- **Disrupts:** Bronze industry collapse, copper/tin trade routes less important
- **New Problems:** Bronze-based economies crash, military parity increases warfare
- **Gameplay:** Democratization of tools, but bronze-dependent cities suffer

**Aqueducts / Civil Engineering (~312 BCE Rome)**
- **Adds:** Fresh water to cities, public fountains, baths, sewer systems
- **Disrupts:** Well-diggers/water-carriers lose work, cities can grow beyond river edges
- **New Problems:** Maintenance costs, vulnerability to sabotage, water rights conflicts
- **Gameplay:** Population capacity increases, but infrastructure maintenance burden

**Concrete / Advanced Construction (~200 BCE Rome)**
- **Adds:** Large-scale architecture (domes, bridges, harbors), permanent roads
- **Disrupts:** Traditional wood/stone construction methods, local material economies
- **New Problems:** Standardization reduces local craftsmen, resource extraction intensifies
- **Gameplay:** Monumental building possible, but requires material supply chains (sand, gravel, stone - see Local Resources section)
- **Local Resource Impact:** Creates demand for sand/gravel extraction - heavy materials that must be sourced locally due to transportation costs

**Paper (~105 CE China)**
- **Adds:** Cheaper writing material, widespread literacy, bureaucracy scale
- **Disrupts:** Parchment/papyrus industries, scribal monopolies weaken
- **New Problems:** Information overload, forgery easier, fire risk in libraries
- **Gameplay:** Administrative efficiency, but document storage becomes critical

#### Medieval Era

**Stirrup (~300 CE)**
- **Adds:** Heavy cavalry dominance, feudal military system
- **Disrupts:** Infantry-based armies less effective, power shifts to mounted nobles
- **New Problems:** Feudalism entrenches class system, peasant armies obsolete
- **Gameplay:** Military reorganization required, social structure changes

**Printing Press (~1440 CE)**
- **Adds:** Mass communication, widespread literacy, scientific knowledge sharing
- **Disrupts:** Scribal profession collapse, church information monopoly broken
- **New Problems:** "Fake news" (propaganda), censorship challenges, copyright concepts
- **Gameplay:** Rapid cultural changes, literacy enables new industries but threatens authorities

**Gunpowder Weapons (~1500 CE in Europe)**
- **Adds:** Infantry can defeat cavalry/armor, fortification innovations, mining
- **Disrupts:** Knights/castles obsolete, feudal military system collapses
- **New Problems:** Warfare casualties skyrocket, arms races, civilian casualties
- **Gameplay:** Military restructuring mandatory, castle-towns vulnerable

#### Industrial Revolution

**Steam Power (~1712 Newcomen, 1769 Watt)**
- **Adds:** Factories, railways, steamships, mechanized production
- **Disrupts:** Artisan/craft industries collapse, rural employment drops, water power obsolete
- **New Problems:** Urban overcrowding, factory accidents, child labor, air pollution
- **Gameplay:** Massive urbanization, but slums and labor unrest emerge

**Railways (~1825)**
- **Adds:** Fast long-distance travel/trade, time zones, national markets
- **Disrupts:** Canal systems obsolete, coaching inns/stables close, rural isolation increases
- **New Problems:** Railroad monopolies, rural depopulation, time standardization conflicts
- **Gameplay:** Economic boom for connected cities, decline for bypassed towns

**Telegraph (~1837)**
- **Adds:** Near-instant long-distance communication, coordinated business/military
- **Disrupts:** Messenger services obsolete, information asymmetry reduces
- **New Problems:** Communication dependency, wire-tapping/espionage
- **Gameplay:** Economic coordination improves, but infrastructure vulnerability

**Electric Grid (~1882 Edison)**
- **Adds:** Interior lighting, power tools, extended work hours, refrigeration
- **Disrupts:** Gas lighting industry, ice harvesting, daytime-only factories
- **New Problems:** Electrification costs, grid maintenance, power monopolies
- **Gameplay:** 24-hour economy possible, but requires infrastructure investment

**Telephone (~1876)**
- **Adds:** Voice communication, business coordination, emergency response
- **Disrupts:** Telegraph decline, messenger services, face-to-face business norms
- **New Problems:** Privacy concerns, phone line infrastructure, operator employment
- **Gameplay:** Real-time business coordination, but infrastructure sprawl

**Automobile (~1908 Model T mass production)**
- **Adds:** Personal mobility, suburbs possible, road freight, drive-in businesses
- **Disrupts:** Railroads decline, horse industries collapse, blacksmiths obsolete, urban density changes
- **New Problems:** Road infrastructure costs, traffic jams, parking demands, air pollution, pedestrian deaths
- **Gameplay:** MASSIVE disruption - suburban sprawl, downtown decline, oil dependency
- **Note:** This is perhaps the biggest single urban disruption in history

**Assembly Line (~1913 Ford)**
- **Adds:** Mass production, affordable consumer goods, standardization
- **Disrupts:** Craft manufacturing, skilled labor value drops, small shops uncompetitive
- **New Problems:** Worker alienation, unemployment of craftsmen, monopoly formation
- **Gameplay:** Consumer economy, but small business collapse

#### Modern Era

**Commercial Aviation (~1930s-1950s)**
- **Adds:** Fast long-distance travel, global tourism, air freight
- **Disrupts:** Ocean liner industry collapse, train travel decline
- **New Problems:** Airport infrastructure costs, noise pollution, crashes
- **Gameplay:** Global connectivity, but local travel industries suffer

**Interstate Highway System (~1956 in US)**
- **Adds:** Fast long-distance driving, trucking industry, road trips
- **Disrupts:** Small-town main streets bypassed, passenger rail nearly extinct, downtowns decline
- **New Problems:** Urban sprawl accelerates, car dependency, strip mall culture
- **Gameplay:** Bedroom communities viable (the game's starting scenario!), but downtown death
- **Local Resource Impact:** ENORMOUS concrete and gravel demand during construction - towns with gravel pits experience temporary construction boom, environmental impact of mass extraction

**Television (~1950s mass adoption)**
- **Adds:** Mass media, advertising reach, instant news
- **Disrupts:** Radio/newspaper dominance, movie theaters decline, local entertainment
- **New Problems:** Cultural homogenization, advertising manipulation, sedentary lifestyle
- **Gameplay:** Retail advertising changes, entertainment industries shift

**Computers / Personal Computers (~1970s-1980s)**
- **Adds:** Information processing, automation, office productivity
- **Disrupts:** Typist/calculator jobs, manual record-keeping, many clerical roles
- **New Problems:** Digital divide, e-waste, office reorganization
- **Gameplay:** White-collar employment changes, education demands shift

**Internet / E-commerce (~1990s-2000s)**
- **Adds:** Global communication, online shopping, remote work, digital services
- **Disrupts:** Physical retail (especially books, music, video), travel agents, newspaper classifieds, local monopolies
- **New Problems:** Main street death, warehouse sprawl, delivery traffic, local tax base erosion
- **Gameplay:** Strip malls struggle, malls die, big box stores + Amazon dominate
- **Note:** This is the disruption the player would be managing in late-game

**Mobile Internet / Smartphones (~2007+)**
- **Adds:** Constant connectivity, gig economy, app-based services
- **Disrupts:** Desktop computing, dedicated devices (cameras, GPS, etc.), taxi industry
- **New Problems:** Attention economy, gig worker exploitation, surveillance capitalism
- **Gameplay:** Retail shifts again (mobile-first), delivery demand increases

**COVID-19 Pandemic (~2020)**
- **Adds:** Remote work normalization, delivery infrastructure, outdoor dining
- **Disrupts:** Office buildings, downtown lunch spots, movie theaters, urban density appeal
- **New Problems:** Office vacancy crisis, downtown dead zones, housing market chaos
- **Gameplay:** Unexpected disruption that revalues everything (suburbs > urban, delivery > retail)

### Design Implications

**Tech Advances Should Require Adaptation:**
- New tech doesn't just unlock buildings - it makes old buildings less viable
- Player must decide: resist change or embrace it?
- Unemployment and economic transitions are part of the challenge
- Some cities will thrive, others will decline based on how they adapt

**Examples in Gameplay:**
- Research automobiles → Suburbs become viable, but downtown stores lose customers
- Research internet → Can build distribution centers, but malls start struggling
- Research railroads → Cities on the line boom, others become backwaters

**The Player's Job:**
- Manage transitions (retrain workers, zone for new industries)
- Decide what to preserve (historic downtown) vs. what to let go
- Deal with stranded assets (empty factories, obsolete infrastructure)
- Balance progress with disruption pain

---

## Local Resources and Transportation Economics

### The Weight Problem: Not All Resources Are Equal

Some resources are **expensive to transport** relative to their value. This creates natural local monopolies and extraction economies.

**Heavy/Bulky Resources (Local Economies):**
- **Gravel and Sand:** Almost every county has gravel pits and sand extraction
  - Essential for roads, concrete, construction
  - Heavy and bulky - transportation costs exceed material value beyond ~50 miles
  - Creates local extraction industries that can't be easily replaced by distant sources
  - Example: A gravel pit 5 miles away is always cheaper than one 100 miles away, regardless of quality
- **Stone/Quarries:** Building stone, crushed rock for aggregate
- **Clay/Bricks:** Clay pits near brick factories (why brickworks were local)
- **Lumber:** Dense forests create local lumber economies (before railroads)
- **Water:** Ultimate local resource - aqueducts, wells, reservoirs

**Valuable/Lightweight Resources (Global Trade):**
- **Gold, gems, spices:** High value-to-weight ratio, worth transporting globally
- **Copper, tin (Bronze Age):** Valuable enough to create long-distance trade networks
- **Oil:** High energy density, worth building pipelines and shipping globally
- **Manufactured goods:** Value-added products worth shipping

### Gameplay Implications

**Resource Extraction Creates Local Wealth:**
- Town with gravel pit has construction material advantage
- Nearby cities must buy from you (or develop their own pit)
- Transportation tech (railroads, trucks) extends the range but doesn't eliminate local advantage
- Player must balance: extract resources locally (jobs, revenue) vs. environmental impact

**Zoning Conflicts:**
- Gravel pit is noisy, dusty, truck traffic
- Creates jobs but reduces nearby residential property values
- Do you zone extraction near existing neighborhoods or force it to edges?
- Exhausted pits: what do you do with a depleted gravel pit? Fill it in? Lake? Landfill?

**Tech Tree Integration:**
- **Concrete (200 BCE):** Requires sand and gravel - creates extraction economy
- **Roads:** Gravel roads need gravel (obvious but creates ongoing demand)
- **Railroads:** Require ballast (crushed stone) - massive gravel demand during construction
- **Interstate Highway System (1956):** ENORMOUS concrete and gravel demand
  - Towns with gravel pits boom during highway construction
  - Creates temporary construction economy
  - Environmental impact of mass extraction

**Economic Geography:**
- Some towns succeed because they're sitting on valuable local resources
- Others must import basic materials at higher cost
- Transportation infrastructure changes which resources are "local enough" to be viable
- Player decisions about where to allow extraction shape long-term development

### Design Philosophy: Resources as Geographic Destiny

Cities don't just appear anywhere - they're shaped by:
- Access to water (rivers, lakes, aquifers)
- Access to building materials (stone, timber, gravel)
- Access to trade routes (natural harbors, river crossings)
- Access to resources (coal, iron, farmland)

**The game should reflect this:**
- Starting map has natural resource deposits (gravel, stone, good farmland)
- Player discovers resources through surveying or development
- Early decisions about extraction create path dependencies
- A town built near quarries develops differently than one near forests
- Transportation tech doesn't eliminate geography - it modifies it

---

## Impossible Cities: Technology and Sustained Intervention

### The Core Insight: Some Cities Require Constant Input to Survive

Traditional city builders treat all locations as equally viable. But real cities often exist in places that require **constant technological and economic intervention** to remain habitable. Remove that intervention, and the city would collapse.

### Cities That Shouldn't Exist (But Do)

**Desert Cities (Phoenix, Las Vegas, Dubai):**
- **The Problem:** Literally no local water supply
- **The Solution:** Massive aqueducts, dams, water imports (Colorado River, etc.)
- **The Reality:** Would collapse in months without external water
- **Why They Exist:** Cheap land, sunshine, mild winters, post-AC population boom
- **Ongoing Cost:** Water imports, AC infrastructure, constant energy demand
- **Vulnerability:** Water rights conflicts, drought, climate change

**Hurricane Zones (New Orleans, Miami, Houston, Tampa):**
- **The Problem:** Periodic destruction from hurricanes and flooding
- **The Solution:** Levees, sea walls, federal disaster relief, subsidized flood insurance
- **The Reality:** Rebuild cycle depends on external federal funding
- **Why They Exist:** Ports, ocean access, tourism, mild winters, established economies
- **Ongoing Cost:** Hurricane insurance, periodic rebuilds, evacuation infrastructure
- **Vulnerability:** Rising sea levels, increased hurricane intensity, federal funding cuts

**Northern Cities (Detroit, Minneapolis, Livonia, Buffalo):**
- **The Problem:** Brutal winters, freeze-thaw road destruction, heating costs
- **The Solution:** Massive snow removal, constant road repair, heating infrastructure
- **The Reality:** 2-3x higher infrastructure maintenance than southern cities
- **Why They Exist:** Great Lakes shipping, fresh water abundance, historical industry, mineral resources
- **Ongoing Cost:** Snow removal, road maintenance, heating fuel, salt damage
- **Vulnerability:** Deindustrialization removes economic advantages, climate makes it hard to attract new residents

**Earthquake Zones (San Francisco, Los Angeles, Tokyo):**
- **The Problem:** Catastrophic earthquake risk ("The Big One")
- **The Solution:** Modern seismic construction codes, emergency preparedness
- **The Reality:** One major quake could level the city
- **Why They Exist:** Natural harbors, strategic location, established economies, mild climate
- **Ongoing Cost:** Expensive building codes, earthquake insurance, preparedness infrastructure
- **Vulnerability:** The catastrophic risk never goes away, only gets mitigated

**Flood Plains (Most River Cities):**
- **The Problem:** Rivers flood regularly - that's what they do
- **The Solution:** Levees, pumps, flood control systems, floodplain management
- **The Reality:** Fighting against natural hydrology requires constant maintenance
- **Why They Exist:** River transportation, fertile soil, water access, historical trade routes
- **Ongoing Cost:** Levee maintenance, pump operation, periodic flood damage
- **Vulnerability:** Climate change increases flood frequency, aging infrastructure

### Why Do These Cities Persist?

**1. Path Dependency - It's Already There:**
- Moving an entire city is harder than maintaining it
- Jobs, families, property ownership, social networks keep people anchored
- Infrastructure investment creates sunk costs
- "We've always lived here" is a powerful force
- Example: New Orleans keeps rebuilding after hurricanes because it's already New Orleans

**2. Specific Advantages Outweigh Costs:**
- Desert cities: Cheap land, solar potential, winter tourism, retiree appeal
- Hurricane zones: Deep water ports, ocean commerce, tourism, fishing, mild winters
- Northern cities: Unlimited fresh water, Great Lakes shipping, no hurricanes, established manufacturing
- Earthquake zones: Natural harbors (SF Bay, Tokyo Bay), strategic military/trade position
- The advantages made the location valuable historically, and path dependency keeps it viable

**3. Technology Enables the Impossible:**
- **Air Conditioning (~1950s):** Made the Sun Belt viable - Phoenix exploded from 100k (1950) to 1.6M (2020)
- **Aqueducts/Dams:** Colorado River Aqueduct (1930s) enables LA and Phoenix to exist at current scale
- **Modern Construction:** Hurricane-resistant buildings, earthquake-resistant skyscrapers
- **Heating Systems:** Make -20°F winters survivable and even comfortable
- **Pumps and Levees:** Keep New Orleans from flooding (most of the time)
- Each technology unlocks previously "impossible" locations

**4. Externalized Costs (Hidden Subsidies):**
- **Federal Flood Insurance:** Subsidizes coastal development that private market wouldn't insure
- **Disaster Relief (FEMA):** Makes hurricane rebuilding "affordable" by spreading costs nationally
- **Water Rights:** 100-year-old agreements give desert cities cheap water from distant sources
- **Highway System:** Connects "impossible" cities to supply chains and markets
- **Agricultural Subsidies:** Make desert farming viable through cheap water and price supports
- Without these subsidies, many "impossible" cities would be economically unviable

### Gameplay Implications

**Early Game - Natural Locations Only:**
- Player can only build where cities naturally thrived historically
- Requirements: Mild climate + water access + farmland + building materials
- Limited viable locations = competition for prime spots
- Examples: River valleys, temperate coasts, near forests/stone quarries

**Technology Unlocks "Hard Mode" Locations:**

*Each tech advance opens new territory but with ongoing costs:*

- **Aqueducts/Canals:** Unlock desert cities
  - Ongoing cost: Water import fees, maintenance
  - Risk: Drought, upstream water rights conflicts

- **Modern Construction (Steel Frame, Concrete):** Unlock earthquake zones, dense urban construction
  - Upfront cost: Higher building codes, expensive materials
  - Risk: Still vulnerable to major quakes

- **Air Conditioning (1950s):** Make extreme heat climates livable
  - Ongoing cost: Massive electricity consumption
  - Risk: Power grid failures during heat waves

- **Levees and Flood Control:** Unlock flood plains and below-sea-level areas
  - Ongoing cost: Pump operation, levee maintenance
  - Risk: Levee failures, Category 5 storms overwhelming systems

- **Federal Systems:** Disaster relief, flood insurance programs
  - Benefit: Makes risky locations financially viable
  - Risk: Political changes, funding cuts, policy shifts

**Ongoing Maintenance Reflects Geographic Reality:**

Instead of one-time disaster events, cities have **permanent cost modifiers** based on location:

- **Desert City:**
  - +200% water costs (importing from distant source)
  - +150% cooling costs (AC essential, not optional)
  - -50% road maintenance (no freeze-thaw damage)
  - Risk: Water source depletion, extended drought

- **Hurricane Zone:**
  - +300% insurance costs
  - Periodic damage every 5-15 years requiring 10-30% of property value to repair
  - +50% building codes (hurricane-resistant construction)
  - -40% heating costs (mild winters)

- **Northern City:**
  - +200% road maintenance (potholes, freeze-thaw cycles)
  - +150% heating costs
  - +100% snow removal operations
  - -90% cooling costs (barely need AC)
  - +100% water infrastructure (pipes must be deep to avoid freezing)

- **Earthquake Zone:**
  - +400% building codes (seismic retrofitting)
  - +250% insurance costs
  - Catastrophic risk: 1-5% chance per decade of major damage
  - Benefit: Often includes mild climate, port access

- **Flood Plain:**
  - +150% flood control infrastructure (levees, pumps)
  - Periodic flooding damage
  - +100% insurance costs
  - Benefit: Fertile soil, river transportation, water access

**The Collapse Question - When Systems Fail:**

What happens when the technology or subsidy that enables your city disappears?

- **Desert city loses water rights:** Immediate crisis, population must evacuate or ration severely
- **Hurricane city loses federal disaster relief:** Rebuild costs fall on local economy - can they afford it?
- **Northern city loses heating fuel supply:** Winter becomes deadly, emergency evacuations
- **Earthquake hits unprepared city:** Catastrophic damage, decades of rebuilding
- **Levee fails in flood plain city:** New Orleans post-Katrina scenario

**Player decisions:**
- Do you build in the "impossible" location for short-term advantages?
- Can you afford the long-term maintenance costs?
- What's your backup plan if the enabling technology fails?
- Is the location viable without external subsidies?

**Climate Change as Long-Term Disruption:**

Over the course of a 100-year game, climate patterns shift:
- Desert cities: Water sources deplete, heat becomes more extreme
- Hurricane zones: More frequent and intense storms, rising sea levels
- Northern cities: Milder winters (lower costs), but also more freeze-thaw cycles in shoulder seasons
- Flood plains: More frequent flooding, harder to maintain levees
- Some "impossible" locations become MORE impossible over time

### Design Philosophy: Cities as Sustained Systems, Not Static Objects

**Traditional City Builder:**
- Build city → Optimize layout → City runs forever with minimal input
- Disasters are occasional events that disrupt but don't fundamentally challenge viability
- All locations are roughly equally viable

**TerminalCity Approach:**
- Build city → Constantly sustain it against geographic/economic realities → Adapt as costs change
- "Disasters" are ongoing cost multipliers, not one-time events
- Location choice is a fundamental strategic decision with permanent consequences
- Some cities are only viable within specific technological and economic eras
- When that era ends, the city must adapt, decline, or be abandoned

**The player's job isn't just building - it's sustaining:**
- Can you afford to keep the desert city watered for 50 years?
- Can you weather the hurricane rebuild cycle without bankruptcy?
- Can you maintain infrastructure through brutal winters?
- What happens when the federal subsidy ends?
- Can you adapt when technology changes (climate change, energy transitions)?

**This creates genuine strategic depth:**
- Short-term thinking: Build in the "best" location for current era
- Long-term thinking: Will this location be viable in 50 years?
- Adaptation thinking: Can we transition when conditions change?
- Portfolio thinking: Maybe you need cities in multiple climate zones to spread risk

Cities aren't permanent fixtures - they're **sustained interventions against geography**. The interesting gameplay comes from managing that reality over decades.

---

## Bankruptcy and Decline: Cities That Fail But Don't Disappear

### The Problem with Traditional City Builders

**Traditional Approach:**
- Bankruptcy = Game Over
- City either succeeds (grows, prospers) or fails (you lose)
- No middle ground, no decline management, no recovery arcs

**Reality:**
- Cities go bankrupt and keep existing
- Detroit declared bankruptcy (2013) - 600,000+ people still live there
- They didn't "lose the game" - they adapted to a new, harder reality
- Some cities recover (Pittsburgh post-steel), others stabilize at lower level
- Decline is a **transition state**, not an end state

### What Actually Happens When Cities Go Bankrupt

**Not This:**
- Game over screen
- City ceases to exist
- Everyone evacuates
- Buildings disappear

**This:**
- **Services Get Slashed:**
  - Fewer police officers, slower response times
  - Reduced fire department coverage
  - Street lights turned off to save money (Detroit did this)
  - Parks go unmaintained, become overgrown
  - Libraries close or reduce hours
  - Public transit routes cut

- **Infrastructure Degrades:**
  - Potholes don't get fixed (roads become obstacle courses)
  - Water mains break more often
  - Sewers back up
  - Abandoned buildings become fire hazards
  - Sidewalks crumble

- **Schools Deteriorate:**
  - Larger class sizes (teacher layoffs)
  - Older textbooks, outdated equipment
  - Building maintenance deferred
  - Programs cut (art, music, sports)
  - Quality of education declines

- **Property Values Crash:**
  - Houses sell for $1 in Detroit (literally)
  - Cheap housing attracts people priced out of other cities
  - Artists, startups, low-income families move in
  - Demographic shifts change neighborhood character

- **Population Declines (But Not to Zero):**
  - People with means leave for better opportunities
  - People without means stay (can't afford to move)
  - Committed residents stay (family, homeownership, attachment)
  - Population shrinks 30-50% but stabilizes at new level
  - Detroit: 1.8M (1950) → 600k (2020) - but still a city

- **Neighborhood Triage:**
  - Can't maintain entire city with reduced budget
  - Some neighborhoods get abandoned (Detroit's outer rings)
  - Other neighborhoods get concentrated investment (downtown)
  - "Managed shrinkage" - deliberately let some areas go
  - Creates patchwork: thriving downtown, abandoned suburbs

- **Spiral Dynamics:**
  - Negative: Cut services → worse quality → people leave → less tax revenue → cut more services
  - Positive: Cheap housing → attracts new residents → potential for revival
  - Negative: Degraded infrastructure → can't attract business → fewer jobs → more people leave
  - Positive: Cheap real estate → opportunity for redevelopment, gentrification

### Examples: Cities That Went Bankrupt or Near-Bankrupt

**Detroit (2013 Bankruptcy):**
- Filed largest municipal bankruptcy in US history ($18-20 billion debt)
- Population: 1.8M (1950) → 600k (2020)
- Services slashed: Street lights off, police response slow, 40+ min ambulance wait
- Some recovery: Downtown redevelopment (Quicken Loans HQ, stadiums), but outer neighborhoods still abandoned
- Patchwork city: Thriving downtown core, vast stretches of abandoned houses
- Art scene thrived due to cheap rent (Heidelberg Project, artist lofts)

**Pittsburgh (Post-Steel Collapse, 1980s):**
- Steel industry collapsed, unemployment hit 18%
- Population: 677k (1950) → 300k (2020)
- **Successful Reinvention:** Pivoted to healthcare, education, tech (Carnegie Mellon, UPMC)
- Shed old identity, embraced new economy
- Still smaller than peak, but stable and prosperous
- Lesson: Recovery is possible with economic diversification

**Stockton, California (2012 Bankruptcy):**
- Housing bubble collapse destroyed tax base
- Pension obligations exceeded revenue
- Filed bankruptcy, renegotiated debts
- Emerged 2015 with lower debt burden
- Still struggling but functional

**New York City (Near-Bankruptcy, 1975):**
- "Ford to City: Drop Dead" (famous headline)
- Federal government refused bailout
- City cut services, laid off workers, raised taxes
- Managed to avoid bankruptcy through austerity
- Recovered in 1980s-90s through financial sector boom
- Lesson: Cities can pull back from the brink

### Gameplay Implications for TerminalCity

**Bankruptcy as Transition, Not Game Over:**
- You don't lose when city goes bankrupt - you enter **crisis management mode**
- New challenge: Can you stabilize the decline? Can you recover? Or do you manage contraction?
- Some of the most interesting gameplay happens during decline, not growth

**Budget Crisis Mechanics:**

When revenue < expenses for X consecutive months:
1. **Warning Phase:** Yellow alert, "budget crisis imminent"
2. **Crisis Phase:** Must make cuts to balance budget
3. **Bankruptcy Phase:** If you can't balance, declare bankruptcy

**Forced Triage Decisions:**

You can't maintain everything with 50% budget cut. Choose:

- **Service Cuts:**
  - Cut police? (Crime increases, property values drop further)
  - Cut fire? (Fire spreads faster, insurance costs rise)
  - Cut schools? (Families with children leave)
  - Cut road maintenance? (Infrastructure degrades, businesses leave)
  - Cut parks/libraries? (Quality of life drops, but less immediate impact)

- **Geographic Triage:**
  - Which neighborhoods get services, which get abandoned?
  - Concentrate resources on downtown core + viable neighborhoods?
  - Or spread thin across whole city (everything mediocre)?
  - Detroit model: Let outer rings depopulate, focus on downtown
  - Managed shrinkage: Deliberately plan for smaller city

- **Debt Restructuring:**
  - Declare bankruptcy → Negotiate with creditors
  - Reduce pension obligations (politically painful)
  - Sell city assets (water system, parking, parks)
  - Each choice has consequences

**Population Dynamics During Decline:**

Not everyone leaves at once - different groups respond differently:

- **Leave First:**
  - Wealthy residents (can afford to move)
  - Young professionals (mobile, seeking opportunity)
  - Families with children (concerned about schools)
  - Businesses (following customers)

- **Stay Longer:**
  - Elderly (homeowners, limited mobility, roots)
  - Poor (can't afford to move)
  - Committed residents (emotional attachment, family)
  - Opportunists (artists, entrepreneurs attracted by cheap rent)

- **Move In:**
  - People priced out of expensive cities
  - Artists seeking cheap studio space
  - Startups with low budgets
  - Immigrants (affordable housing)
  - "Urban pioneers" / gentrifiers

**Spiral Management:**

Player must break negative spirals or they compound:

- **Negative Spiral:**
  - Cut police → Crime rises → Property values drop → Tax revenue falls → Cut more police
  - Cut schools → Families leave → Fewer kids → Close schools → Neighborhood dies
  - No maintenance → Potholes → Business leaves → Less revenue → No maintenance

- **Potential Positive Interventions:**
  - Attract new industry (tax breaks, targeted investment)
  - University expansion (stable employer, attracts young people)
  - Transit connection (access to regional job market)
  - Targeted demolition (remove blight, consolidate population)
  - Arts district (cheap rent attracts creatives, becomes destination)

**Recovery Paths:**

Multiple ways to potentially recover from bankruptcy:

1. **Economic Reinvention (Pittsburgh Model):**
   - Identify new industry that fits your assets
   - Partner with universities for research/tech
   - Invest in workforce retraining
   - Shed old industrial identity
   - Takes 10-20 years but can work

2. **Managed Shrinkage (Detroit Model):**
   - Accept you'll be smaller
   - Consolidate population into viable core
   - Demolish abandoned areas or convert to green space
   - Focus resources on what remains
   - Stable but diminished

3. **Bankruptcy as Reset (Stockton Model):**
   - Declare bankruptcy, shed debt
   - Renegotiate pension obligations
   - Start fresh with lower fixed costs
   - Rebuild slowly from stronger foundation

4. **External Rescue:**
   - State/federal bailout (NYC 1975)
   - Major employer relocates to city (Amazon HQ2 style)
   - Natural resource discovery (oil, minerals)
   - Geographic advantage becomes relevant (port, rail hub)

5. **Gentrification/Revival:**
   - Cheap real estate attracts artists/startups
   - Cool factor develops (Brooklyn, Portland)
   - Property values rise, new investment flows in
   - Original residents may get displaced (new problem)

**Victory Conditions Reconsidered:**

Traditional city builder: Grow to X population, Y revenue

TerminalCity alternatives:
- **Survival Victory:** Keep city functional for 50/100 years despite challenges
- **Recovery Victory:** Decline to X, recover to Y (comeback story)
- **Adaptation Victory:** Successfully transition through 3+ economic eras
- **Stability Victory:** Maintain quality of life despite disruptions
- **No Victory:** Just see how long you can keep it going, how you respond to challenges

**Visual Representation of Decline:**

Show the decay, don't hide it:
- Abandoned buildings (broken windows, overgrown yards)
- Dark streets (street lights off)
- Potholes visible on roads
- Graffiti on neglected buildings
- Parks overgrown
- But also: Some neighborhoods still maintained, creating patchwork
- Shows player the consequences of their triage decisions

### Design Philosophy: Decline is Not Failure

**Traditional City Builders:**
- Growth = success, decline = failure
- Optimization puzzle with "correct" answers
- Once solved, stays solved

**TerminalCity:**
- **Decline is a valid game state with interesting decisions**
- Managing bankruptcy is a different challenge than managing growth
- Recovery arcs are dramatic and rewarding
- No "correct" answer - just trade-offs and adaptation
- The city that declines, adapts, and survives is as successful as one that never declined

**The Most Interesting Gameplay:**
- Not building the perfect city
- But managing change, decline, disruption, adaptation
- Wrestling with tough choices (who gets services?)
- Attempting recovery against long odds
- Seeing the long-term consequences of decisions
- The story of a city across 100 years of upheaval

**Player Agency in Crisis:**
- You can't prevent all decline (external forces matter)
- But you can shape HOW the city declines
- And you can attempt different recovery strategies
- The challenge is making the best of bad situations
- Detroit didn't "lose the game" - it's still playing

---

## Learning from Existing Games: Tech Tree Analysis

### Why Analyze Other Games?

Most city builders and strategy games model technology as **purely additive** - each advance unlocks new buildings, units, or bonuses. Real technology is **disruptive** - it creates winners and losers, makes things obsolete, forces adaptation.

Analyzing what existing games do (and don't do) helps TerminalCity identify opportunities to model disruption and change more realistically.

---

### Civilization Series (Civ 5/6 Tech Trees)

Civilization has one of the most famous tech trees in gaming, spanning ancient era to future. Each tech unlocks units, buildings, wonders, and tile improvements. But it almost never models obsolescence or disruption.

#### Sample Technologies and Analysis:

**Animal Husbandry (Ancient Era)**
- **Unlocks in Game:** Pasture improvement (horses, cattle, sheep), Horseman unit, Circus (happiness building)
- **What It Actually Disrupted:** Hunter-gatherer lifestyle, nomadic cultures, natural predator balance
- **What Game Ignores:**
  - Doesn't make hunting camps less effective
  - No disease transmission from animals to humans (historical plagues)
  - No grazing land conflicts with farmers
  - Pure bonus - no trade-offs

**Bronze Working (Ancient Era)**
- **Unlocks in Game:** Spearman unit, can clear jungle, can see iron on map
- **What It Actually Disrupted:** Stone tool makers obsolete, stone age weapons worthless, copper-only regions disadvantaged in warfare
- **What Game Ignores:**
  - Stone age units don't become obsolete (you can still build Warriors)
  - No trade network dependency (tin is rare, copper more common - need both for bronze)
  - No economic collapse for regions without tin access
  - Clearing jungle with bronze tools is anachronistic (steel tools for that)

**Iron Working (Classical Era)**
- **Unlocks in Game:** Swordsman unit, can see iron on map, can clear marsh
- **What It Actually Disrupted:** Bronze industry collapse (Bronze Age collapse ~1200 BCE), copper/tin trade routes diminished, democratization of warfare (iron more available than bronze)
- **What Game Ignores:**
  - Bronze units don't become obsolete or more expensive
  - No economic crash in bronze-producing regions
  - Iron is just another resource, not a paradigm shift
  - Military parity increases (anyone can make iron weapons) but game doesn't model this

**Currency (Classical Era)**
- **Unlocks in Game:** Market building (+gold), Caravansary, enables gold trade routes
- **What It Actually Disrupted:** Barter economies, local self-sufficiency, debt relationships
- **What Game Ignores:**
  - You already have gold before researching currency (abstraction issue)
  - No transition from barter to money economy
  - No inflation mechanics
  - No debt/banking systems enabled

**Printing Press (Renaissance Era)**
- **Unlocks in Game:** +1 culture from trading posts, enables cultural buildings
- **What It Actually Disrupted:** Scribal profession collapse, church information monopoly broken, enabled Reformation and scientific revolution
- **What Game Ignores:**
  - No scribes to make obsolete (not a unit/building)
  - No church authority mechanic to challenge
  - Cultural impact vastly understated (just +1 culture)
  - No "fake news" / propaganda mechanics
  - One of history's most disruptive technologies, treated as minor bonus

**Steam Power (Industrial Era)**
- **Unlocks in Game:** Ironclad ship, Factory building (+production), can build railroads after researching Railroad
- **What It Actually Disrupted:** Artisan industries, water-powered mills, sail-based shipping, rural employment patterns
- **What Game Ignores:**
  - Artisan/windmill/watermill buildings don't become obsolete or less effective
  - No urban migration / slum mechanics
  - No worker displacement or labor unrest
  - Factory is just better than workshop - no transition pain
  - No pollution until much later (Coal Plant)

**Railroad (Industrial Era)**
- **Unlocks in Game:** Can build railroads on tiles (+movement, +production on tiles with mines/quarries)
- **What It Actually Disrupted:** Canals obsolete, coaching inns/stables close, rural isolation increases, time zones standardized
- **What Game Ignores:**
  - Canal cities don't decline
  - No railroad monopoly mechanics
  - Coaches/caravans don't become obsolete
  - Towns not on railroad lines don't suffer
  - Just pure benefit - faster movement, more production

**Electricity (Modern Era)**
- **Unlocks in Game:** Stock Exchange, Hydro Plant, enables other modern buildings
- **What It Actually Disrupted:** Gas lighting industry, ice harvesting, daytime-only factories, changed labor patterns (24-hour economy)
- **What Game Ignores:**
  - No gas lighting to make obsolete
  - No 24-hour economy mechanics
  - No labor shift changes
  - No electrification cost (just build power plant, everything works)
  - Vastly understates the transformation

**Automobile (Modern Era)**
- **Unlocks in Game:** Tank unit, Modern Armor unit later
- **What It Actually Disrupted:** Railroads, horse industries (blacksmiths, stables, farriers), urban density, downtown retail
- **What Game Ignores:**
  - THIS IS THE BIG ONE - perhaps the most urban-transforming technology in history
  - Enables suburbs (not in game)
  - Kills downtown retail (not in game)
  - Creates parking demand (not in game)
  - Creates traffic jams (not in game)
  - Air pollution (not really modeled)
  - Railroad travel declines (doesn't happen in game)
  - Horse stables don't close
  - **Treated as military tech only, ignoring massive civilian impact**

**Computers (Information Era)**
- **Unlocks in Game:** Research Lab (+science), various modern units, can build spaceship parts
- **What It Actually Disrupted:** Typist/calculator jobs, manual record-keeping, many clerical professions
- **What Game Ignores:**
  - No clerical jobs to eliminate
  - No automation of labor
  - No digital divide
  - Just another science boost

**Internet (Information Era) - Civ 6**
- **Unlocks in Game:** Research bonuses, culture bonuses, policy cards
- **What It Actually Disrupted:** Physical retail, newspapers, travel agents, local monopolies, physical media (books, music, video)
- **What Game Ignores:**
  - Possibly the second-biggest urban disruptor after automobiles
  - Main street retail doesn't decline
  - Newspaper buildings not affected
  - No e-commerce mechanics
  - No remote work enabling
  - No information asymmetry reduction
  - **One of the most transformative technologies, treated as minor bonus**

#### Civ's Pattern: Additive Only

**What Civ Does Well:**
- Clear tech progression with prerequisites
- Unlocks feel meaningful (new units, buildings, abilities)
- Historical flavor and education value
- Strategic choices (which tech path to prioritize)

**What Civ Never Models:**
- **Obsolescence:** Old buildings/units don't become useless or expensive to maintain
- **Disruption:** New tech doesn't make old economic models collapse
- **Unemployment:** Workers displaced by tech don't become a problem
- **Transition Costs:** No pain of adapting to new technology
- **Geographic Winners/Losers:** Railroad cities don't boom while canal cities decline
- **Stranded Assets:** Your investment in windmills doesn't lose value when you get steam power

**Why This Matters for TerminalCity:**

Civ shows the **default gaming approach** - tech is a reward ladder. TerminalCity's opportunity is to show tech as **disruption + adaptation**.

---

### SimCity Series (Primarily SimCity 4 / 2013)

SimCity is more focused on urban management than Civilization, but it still mostly treats "progress" as additive unlocks based on population/money/time.

#### SimCity "Tech" Progression:

SimCity doesn't have a formal tech tree, but it has **population-gated unlocks** - certain buildings, zones, and options only become available as your city grows or progresses through time.

**Early Game (Small Town):**
- **Unlocks:** Basic zones (residential, commercial, industrial), basic services (fire, police, water, power)
- **What SimCity Ignores:** Early industry (agriculture, blacksmiths, small mills) not really modeled - game starts at "post-industrial small town" already

**Medium Growth (City):**
- **Unlocks:** Upgraded zones (denser buildings), health clinics, schools, buses, more power plants
- **What SimCity Ignores:** No disruption of small businesses by bigger businesses, no economic transitions

**Large Growth (Metropolis):**
- **Unlocks:** High-density zones, subway, airports, advanced services, landmarks
- **What SimCity Ignores:** No suburban sprawl mechanics really (suburbs are just low-density), no downtown decline from auto dominance

**Industrial Transition:**
- **SimCity 4 Had This:** Dirty industry → manufacturing → high-tech
- **What It Modeled:** Pollution decreases, educated workforce needed
- **What It Ignored:**
  - Factory closures don't cause unemployment crisis
  - Workers magically retrain for high-tech jobs
  - No stranded assets (old factories just disappear or convert)
  - No rust belt decline mechanics

**Transportation Evolution:**
- **Progression:** Roads → Avenues → Highways → Subway → Airport
- **What It Models:** Traffic reduction, faster travel
- **What It Ignores:**
  - Highways don't kill downtown retail (huge real-world effect)
  - No induced demand (more roads = more traffic)
  - Subway doesn't kill surface transit
  - Airport doesn't affect local noise/pollution much
  - No railroad decline when highways arrive

**Power Plants:**
- **Progression:** Coal → Oil → Natural Gas → Nuclear → Wind/Solar
- **What It Models:** Pollution reduction, efficiency
- **What It Ignores:**
  - Coal plants don't become economically obsolete (you choose to demolish them)
  - No stranded assets from switching energy sources
  - No worker retraining needed
  - No political resistance to plant closures

**Education:**
- **Unlocks:** Elementary → High School → College → University → Library
- **What It Models:** Educated workforce enables high-tech industry
- **What It Ignores:**
  - No obsolescence of low-education jobs
  - Everyone magically finds work at their education level
  - No brain drain (educated leave for better cities)
  - No student debt mechanics

**Landmarks/Specializations (SimCity 2013):**
- **Gambling City:** Build casinos, attract tourists
- **Mining City:** Extract resources, ship to other cities
- **Electronics City:** Build consumer electronics
- **What It Models:** Economic specialization
- **What It Ignores:**
  - Boom-bust cycles (mining city doesn't collapse when ore depleted)
  - Gambling doesn't displace local retail (net negative locally)
  - No long-term consequences of specialization
  - No stranded workers when industry changes

#### SimCity's Pattern: Growth Treadmill

**What SimCity Does Well:**
- Immediate feedback on traffic, pollution, happiness
- Zoning and density mechanics
- Service coverage visualization
- Trade-offs in budget management
- Organic city growth based on your decisions

**What SimCity Never Models:**
- **Decline:** Cities only grow, never shrink (unless you're failing)
- **Obsolescence:** Buildings don't become outdated or less desirable over time
- **Economic Disruption:** New industries don't kill old ones
- **Stranded Assets:** Your investment in coal plants doesn't hurt you when switching to solar
- **Suburban Sprawl Economics:** Low-density zones don't bankrupt your city with infrastructure costs (they should)
- **Downtown Death:** Highways and malls don't kill main street (they do in real life)

**Why This Matters for TerminalCity:**

SimCity shows the **city builder default** - focus on growth and optimization. TerminalCity's opportunity is showing **change, adaptation, decline, and recovery**.

---

### Banished (2014)

Banished is different - it's a **survival city builder** about managing a small medieval/colonial settlement. It focuses on resource scarcity, harsh winters, and the fragility of communities. Much more relevant for TerminalCity's earlier scenarios.

#### Banished's "Tech" System:

Banished doesn't have formal technologies - instead you have **buildings that enable new capabilities**. Progress is gated by resources and population, not research.

**Starting Resources:**
- Small group of families, some tools, some food, some firewood, some clothes
- Must survive winter and establish sustainable resource loops

**Key Buildings and What They Enable:**

**Gathering Hut:**
- **Unlocks:** Gather food from forest (berries, nuts, roots)
- **Real-Life Parallel:** Hunter-gatherer survival
- **What Banished Models:** Sustainable but low-yield food source
- **What It Ignores:** Overharvesting reducing yields over time (it's infinite)

**Fishing Dock:**
- **Unlocks:** Fish from rivers/lakes
- **Real-Life Parallel:** Fishing as food source
- **What Banished Models:** Need to place near water
- **What It Ignores:** Overfishing reducing yields, fish population dynamics

**Crop Fields:**
- **Unlocks:** Agriculture - plant seeds, harvest crops
- **Real-Life Parallel:** Agricultural revolution
- **What Banished Models:**
  - Seasonal cycle (plant spring, harvest fall)
  - Crop rotation needed (yields drop without rotation)
  - Weather affects yields
  - Labor intensive (need workers)
- **What It Gets Right:** Agriculture is risky (bad harvest = famine)
- **What It Ignores:** Doesn't make gathering obsolete (both remain viable)

**Pasture:**
- **Unlocks:** Raise animals (chickens, sheep, cattle)
- **Real-Life Parallel:** Animal husbandry
- **What Banished Models:**
  - Need to acquire animals through trade
  - Breeding takes time
  - Provides food (meat) and resources (leather, wool)
- **What It Gets Right:** Sustainable animal populations, long-term investment
- **What It Ignores:** Disease transmission, grazing land conflicts

**Orchards:**
- **Unlocks:** Fruit trees (apples, peaches, pears)
- **Real-Life Parallel:** Orchard cultivation
- **What Banished Models:**
  - Takes years to become productive (trees must mature)
  - Long-term stable food source
- **What It Gets Right:** Multi-year investment payoff
- **What It Ignores:** Trees don't age out or die after decades

**Blacksmith:**
- **Unlocks:** Make tools from iron
- **Real-Life Parallel:** Metalworking
- **What Banished Models:**
  - Need iron ore (finite resource from mines)
  - Need coal for fuel
  - Tools wear out and must be replaced (great mechanic!)
- **What It Gets Right:** **Tool degradation is a core mechanic** - without tools, productivity drops dramatically
- **What It Ignores:** Blacksmith doesn't unlock better tools, just maintains existing ones

**Tailor:**
- **Unlocks:** Make clothing from leather/wool
- **Real-Life Parallel:** Textile production
- **What Banished Models:**
  - Clothing wears out (must replace)
  - Cold winters require warm coats
  - Leather from hunters, wool from sheep
- **What It Gets Right:** **Clothing degradation** - people die in winter without warm clothes
- **What It Ignores:** Doesn't model textile tech improvements (spinning wheel, loom)

**Trading Post:**
- **Unlocks:** Trade with merchants for resources/animals/seeds you don't have
- **Real-Life Parallel:** Long-distance trade
- **What Banished Models:**
  - Need surplus goods to trade
  - Can acquire resources not available locally (cattle, seeds, etc.)
  - Merchants visit periodically (not on demand)
- **What It Gets Right:** Trade as essential for diversification
- **What It Ignores:** No trade route building, no trade network dynamics

**School:**
- **Unlocks:** Educate children to become more productive adults
- **Real-Life Parallel:** Education system
- **What Banished Models:**
  - Children must spend time in school (not working)
  - Educated adults produce more resources
  - Trade-off: short-term labor loss for long-term gain
- **What It Gets Right:** Education as investment with delayed payoff
- **What It Ignores:** No specialization (doctor, teacher, engineer)

**Hospital:**
- **Unlocks:** Treat illness, reduce death rate
- **Real-Life Parallel:** Healthcare
- **What Banished Models:**
  - Requires herbs (gathered from herbalist)
  - Reduces illness deaths
- **What It Ignores:** Doesn't model specific diseases, epidemics, or medical tech advances

**Town Hall:**
- **Unlocks:** View statistics, manage priorities
- **Real-Life Parallel:** Governance/administration
- **What Banished Models:** Information visibility
- **What It Ignores:** No actual governance mechanics (taxes, laws, policies)

#### Banished's Core Mechanics (Not Buildings):

**Death Spiral Dynamics:**
- **Famine:** Bad harvest → people starve → fewer workers → can't plant enough → more famine
- **Cold:** No firewood → people freeze → fewer workers → can't gather wood → more freezing
- **Tool Death:** No tools → low productivity → can't mine iron → can't make tools → productivity collapses
- **Aging Population:** If you don't manage birth rate, everyone gets old at once → mass death → population collapse

**What Banished Gets RIGHT:**
- **Degradation:** Tools and clothing wear out - you must constantly replace them
- **Seasons:** Winter is deadly - you must prepare or people die
- **Fragility:** One bad season can collapse your settlement
- **Resource Loops:** Everything interconnected (iron → tools → productivity → food → survival)
- **No Pure Growth:** Can't just build bigger - must balance resources carefully
- **Demographic Management:** Birth rate, death rate, age distribution all matter
- **No Tech Ladder:** It's not about unlocking new tech, it's about surviving with what you have

**What Banished Doesn't Model:**
- **Tech Progression:** No advancement beyond medieval tools (no industrial revolution)
- **Economic Disruption:** New buildings don't make old ones obsolete
- **Trade Networks:** Trading post is isolated, not part of regional economy
- **External Forces:** No wars, no plagues (just starvation/cold/old age), no economic changes
- **Time Period:** Locked in vague medieval/colonial era

#### Banished's Pattern: Survival, Not Progress

**What Makes Banished Different:**
- It's about **sustaining**, not growing
- Focus on **resource loops** and **fragility**
- Death and failure are always close
- Degradation is core (tools, clothing wear out)
- Seasons and weather create rhythm and challenge
- Population dynamics matter (age, education, reproduction)

**What Banished Teaches TerminalCity:**
- **Degradation Mechanics Work:** Tools/clothing wearing out creates ongoing challenge
- **Fragility Creates Tension:** Being close to failure is engaging
- **Resource Loops:** Everything interconnected (iron → tools → food → survival)
- **Seasons/Cycles:** Predictable rhythms with variation keep gameplay interesting
- **No Pure Growth:** Balance and sustainability > just building bigger

**How TerminalCity Could Use This:**
- Early scenarios (pre-1800) could have Banished-like survival elements
- Infrastructure degradation (roads, water systems) needs ongoing maintenance
- Seasonal challenges (winter road damage in Livonia)
- Resource loops (gravel pits → concrete → roads → maintenance)
- Fragility: Cities can fail if key systems collapse
- But add: Economic disruption, tech changes, external forces Banished lacks

---

### Victoria 3: The Gold Standard for Economic Disruption

Victoria 3 (2022) is a grand strategy game covering 1836-1936. While not a city builder, it's the **best example** of a game that models economic disruption, technological obsolescence, worker displacement, and political reactions to change. TerminalCity should study its systems closely.

#### What Victoria 3 Does Differently

**Core Philosophy: Pops Are the Economy**

Unlike most strategy games where resources flow abstractly, Victoria 3 models **population groups (Pops)** as the economic actors:
- Each Pop has: Job, wealth, education level, political beliefs, standard of living
- Pops work in buildings (farms, factories, mines, offices)
- Pops consume goods based on their wealth/class
- Pops vote, protest, or rebel based on their political beliefs
- **When tech changes, Pops lose jobs → become unemployed → radicalize → political crisis**

This creates a **human cost** to economic change that most games ignore.

#### The Production Method System

This is Victoria 3's killer feature for modeling disruption:

**How It Works:**
- Each building (farm, factory, mine) has multiple **production methods**
- Different methods use different inputs, outputs, and workers
- Technology unlocks new production methods
- New methods are usually more efficient BUT employ fewer workers

**Example: Textile Mill**

**Production Method 1: Home Workers (1836, starting tech)**
- Inputs: Fabric (from local weavers)
- Outputs: Clothes
- Workers: 5,000 Craftsmen (artisan weavers)
- Efficiency: Low
- Wages: Decent (skilled labor)

**Production Method 2: Workshops (1840s, early mechanization)**
- Inputs: Fabric, Tools
- Outputs: More Clothes
- Workers: 3,000 Craftsmen + 2,000 Laborers (unskilled)
- Efficiency: Medium
- Wages: Lower (deskilling)
- **Disruption:** 2,000 craftsmen lose jobs, become laborers or unemployed

**Production Method 3: Mechanized Factory (1860s, industrial revolution)**
- Inputs: Fabric, Coal (steam power)
- Outputs: Much More Clothes
- Workers: 500 Machinists (skilled) + 1,500 Laborers
- Efficiency: High
- **Disruption:** 3,000 more workers displaced
- **Political Impact:** Mass unemployment → radicalization → socialist movement grows

**Production Method 4: Automated Factory (1900s, assembly line)**
- Inputs: Fabric, Electricity, Tools
- Outputs: Massive Clothes Production
- Workers: 200 Engineers + 300 Machinists
- Efficiency: Very High
- **Disruption:** 1,500 more workers displaced
- **But:** Cheaper clothes raise standard of living for consumers

**The Player's Dilemma:**
- Do you adopt new production method?
  - YES: More efficient, cheaper goods, but mass unemployment
  - NO: Less competitive, lose money, but keep workers employed
- You can't prevent tech change forever (competitors will adopt it)
- But you can manage the transition speed and provide safety nets

#### Political Systems: When Workers Fight Back

Victoria 3's political system reacts to economic disruption:

**Interest Groups:**

Each pop belongs to an interest group based on class/profession:
- **Landowners:** Aristocrats, want protectionism, oppose industry
- **Capitalists:** Factory owners, want free trade and low taxes
- **Petite Bourgeoisie:** Shopkeepers and craftsmen, want protection from big business
- **Trade Unions:** Industrial workers, want labor laws and welfare
- **Intelligentsia:** Professionals/academics, want education and reform
- **Armed Forces:** Military, want high military spending
- **Rural Folk:** Farmers, want cheap tools and high food prices
- **Devout:** Religious, want religion in government

**What Happens When You Disrupt the Economy:**

**Scenario: You mechanize textile industry**

1. **5,000 Craftsmen lose jobs**
2. **Craftsmen pop wealth drops** (unemployed have no income)
3. **Craftsmen become radical** (angry about unemployment)
4. **Petite Bourgeoisie interest group loses clout** (fewer members with wealth)
5. **Trade Unions gain members** (displaced craftsmen join unions)
6. **Political movements activate:**
   - Socialists demand welfare programs
   - Conservatives want to protect traditional industries
   - Liberals want free market (let weak industries die)

**Player Must Respond:**

**Option 1: Ignore (Free Market Approach)**
- Unemployed workers stay unemployed or emigrate
- Risk: Revolutions, strikes, political instability
- Benefit: Most efficient economy

**Option 2: Welfare State**
- Enact "Poor Laws" or "Unemployment Insurance"
- Costs money (higher taxes)
- Reduces radicalization
- Benefit: Social stability, slower but managed transition

**Option 3: Protectionism**
- Ban new production methods or foreign competition
- Keeps old jobs alive temporarily
- Costs: Uncompetitive economy, higher prices
- Eventually can't compete, crisis delayed not prevented

**Option 4: Retraining/Migration**
- Invest in education (craftsmen → machinists)
- Encourage migration to growing regions
- Long-term solution but takes time
- Meanwhile: Political instability

**The Meta-Game:**
- You're constantly balancing efficiency vs. social stability
- Every tech advance creates winners and losers
- Losers organize politically and can overthrow you
- No "correct" answer - just trade-offs

#### Standard of Living: Does "Progress" Help People?

Victoria 3 tracks whether economic growth actually improves lives:

**Standard of Living (SoL) Score per Pop:**
- Based on: Wage, cost of goods, access to services
- Can go up OR down during industrialization

**Example Scenarios:**

**Scenario 1: Failed Transition**
- Mechanize factories → Mass unemployment
- Wages drop (labor surplus)
- Standard of Living: ↓↓ (workers worse off)
- Political Result: Revolution

**Scenario 2: Successful Transition**
- Mechanize factories → Unemployment
- But: Cheaper goods (mechanization = efficiency)
- New industries absorb some workers (railways, construction)
- Wages stable, goods cheaper
- Standard of Living: → or ↑ (mixed results)
- Political Result: Grumbling but manageable

**Scenario 3: Inclusive Growth**
- Mechanize factories + Build schools
- Unemployed craftsmen retrain as machinists/engineers
- Wages rise for skilled workers
- Cheaper goods benefit everyone
- Standard of Living: ↑↑ (workers better off)
- Political Result: Stable, progressive politics

**Why This Matters:**
- Shows that "economic growth" ≠ "people better off"
- Forces player to think about distribution, not just GDP
- Political stability depends on SoL, not just total wealth

#### Migration and Demographic Shifts

Victoria 3 models population movement in response to economic change:

**Internal Migration:**
- Pops move from declining regions to booming regions
- Example: Farmworkers leave countryside → Move to industrial cities
- Creates: Urban growth + Rural depopulation
- Visually: City populations explode, rural areas empty out

**Emigration:**
- Pops leave country entirely if SoL is terrible
- Example: Irish famine → Mass emigration to America
- Game shows: Waves of emigrants leaving your country
- Political Impact: Brain drain, population loss, labor shortage

**Immigration:**
- Pops move TO your country if it's prosperous
- Example: America attracts European immigrants
- Game shows: Population boom from immigration
- Political Impact: Nativism, ethnic tensions, but also economic growth

**Why This Matters for TerminalCity:**
- Cities grow not just from birth rate, but from migration
- Economic opportunity attracts people, decline drives them away
- Depopulation is a real consequence of failed economic transitions
- Visual: Neighborhoods empty out when jobs leave (Detroit model)

#### Economic Geography: Winners and Losers

Victoria 3 shows how tech changes which regions thrive:

**Example: Railroad Technology**

**Before Railroads (1840s):**
- River cities thrive (water transport only option)
- Coastal ports dominate trade
- Interior regions isolated, underdeveloped
- Canal cities boom (Erie Canal, etc.)

**After Railroads (1860s+):**
- Railroad hubs explode in population/wealth
- Cities bypassed by rail decline
- Interior regions suddenly viable (can ship goods)
- Canal cities stagnate (slower than rail)
- Coal mining regions boom (fuel for trains)

**Visual Representation:**
- Map colors show prosperity (green = thriving, red = declining)
- See regions shift from green → red when bypassed
- Railroad lines glow on map, showing new trade routes

**Player Decisions:**
- Do you invest in railroad to your region? (expensive)
- Or accept being bypassed and decline? (cheaper but brutal)

#### Concrete Example: The Luddite Problem

**Historical Context:**
- 1811-1816: English textile workers destroyed machines
- Why? Machines were taking their jobs
- "Luddite" became term for opposing technology

**How Victoria 3 Models This:**

1. **You research "Mechanization"**
2. **Textile factories adopt power looms**
3. **10,000 Craftsmen (hand weavers) lose jobs**
4. **Craftsmen become radical (+50% radical):**
   - Join socialist movement
   - Support revolutionary parties
   - Lower loyalty to government

5. **Craftsmen interest group (Petite Bourgeoisie) demands action:**
   - "Ban the machines!" (protectionism)
   - Or: "Provide relief!" (welfare)
   - Or: They'll rebel

6. **You must choose:**
   - **Ignore:** Risk rebellion, but efficient economy
   - **Welfare:** Enact "Poor Laws," costs money, reduces radicalization
   - **Protectionism:** Ban power looms (keeps jobs but uncompetitive)
   - **Suppression:** Use military to crush dissent (authoritarian path)

7. **Consequences play out over years:**
   - If ignored: 1815 Luddite Rebellion event fires
   - If welfare: Radicalization slowly decreases, but high taxes
   - If protectionist: Economy stagnates, eventually must change anyway
   - If suppressed: Short-term peace, long-term resentment

**Why This Is Brilliant:**
- Technology creates a **political crisis**, not just an economic one
- There's no "correct" answer - just trade-offs
- Historical events emerge from systems, not scripted
- Player feels the **human cost** of technological change

#### What TerminalCity Can Learn from Victoria 3

**Core Systems to Adapt:**

**1. Population Has Jobs That Can Disappear**
- Don't abstract employment
- Track: "5,000 factory workers" in specific buildings
- When factory closes: Those 5,000 become unemployed
- Unemployed can't pay rent, can't buy goods → ripple effects

**2. Buildings Have Multiple Production Methods**
- Family farm vs. Commercial farm (different labor, efficiency)
- Corner store vs. Supermarket vs. E-commerce (different workers, scale)
- Artisan workshop vs. Factory vs. Automated factory
- Player sees: "Upgrade to commercial farming? Will displace 500 families"

**3. Technology Creates Winners and Losers**
- Don't just unlock new buildings
- Show who loses when new building arrives
- Blacksmiths → Unemployed when cars arrive
- Downtown shops → Empty when mall opens
- Track these transitions, make them visible

**4. Political Reactions to Economic Change**
- Interest groups: Homeowners (NIMBYs) vs. Renters (YIMBYs)
- When you disrupt economy: Affected groups organize, oppose, vote
- Player must manage political backlash, not just economics
- Can be voted out if you disrupt too fast without support

**5. Standard of Living as Victory Condition**
- Not just "city population" or "tax revenue"
- Track: Are people's lives actually improving?
- You can have "growth" while most residents are worse off
- This creates moral dimension to decisions

**6. Migration as Consequence**
- People move TO opportunity, AWAY FROM decline
- Neighborhood depopulation when factory closes
- Gentrification when area improves (displacement)
- Visual: See population density shift over time

**7. Geographic Winners and Losers**
- Railroad favors some neighborhoods, bypasses others
- Highway kills downtown, boosts suburbs
- Internet favors urban cores (walkable), hurts strip malls
- Show these shifts visually on map

**8. No "Correct" Path**
- Victoria 3 doesn't judge your choices
- Capitalist efficiency vs. Socialist welfare vs. Conservative tradition
- All have trade-offs
- TerminalCity should be similar: No "right" way to manage disruption

#### Scaling to City Builder: What Changes

**Victoria 3 Scale:** Nations (millions of people, abstract)
**TerminalCity Scale:** Single city (thousands of people, visual)

**What to Keep:**
- Pop system (track jobs, wealth, politics)
- Production method changes
- Political interest groups
- Migration and demographic shifts
- Standard of living metrics
- Economic geography (winners/losers)

**What to Change:**
- **More granular:** See individual buildings, not just state-level stats
- **Visual feedback:** See neighborhoods decay, specific buildings close
- **Micro decisions:** Zone this block, place this building, rezone this factory
- **Slower time:** Decades not centuries (1955-2000 is 45 years)
- **Single city focus:** Deep simulation of one place, not entire world

**Example Translation:**

**Victoria 3:**
"Textile industry mechanized. 50,000 craftsmen unemployed across country."
(Abstract, national level)

**TerminalCity:**
"Johnson Textile Mill at 5th & Main adopted power looms. 47 workers laid off. Unemployment in Mill District now 23%. City councilman proposes relief program."
(Specific, visual, local, political)

#### The Vision: Victoria 3's Systems + City Builder's Immersion

**Imagine:**

You're playing TerminalCity, starting 1955:

1. **Your city has downtown shops** (mom & pop stores employing 200 people)
2. **1970: Shopping mall opens** on edge of city (you zoned for it)
3. **Downtown shops start closing** (can't compete with mall)
4. **Unemployment rises** in downtown neighborhood
5. **Property values drop** downtown
6. **Crime increases** (unemployment + blight)
7. **City council meeting:**
   - Business owners demand subsidies to compete
   - Mall developers say "progress can't be stopped"
   - Workers demand retraining programs
   - You must decide: Prop up downtown? Let market decide? Tax mall to fund programs?

8. **1990: Internet arrives**
9. **Now the MALL starts dying** (e-commerce)
10. **Those mall jobs disappear**
11. **Massive parking lot sits empty** (stranded asset)
12. **You must adapt again:** Convert mall to mixed-use? Office park? Demolish?

**This is Victoria 3's disruption systems applied to city building.**

You're not just building - you're **managing constant economic change** and the human/political consequences.

#### Why This Matters

Most city builders are **growth simulators.** You build, optimize, win.

Victoria 3 is a **change simulator.** You adapt, manage crises, make hard choices.

**TerminalCity should be the Victoria 3 of city builders:**
- Economic disruption creates interesting problems
- Technology creates winners and losers
- Politics matters (NIMBY/YIMBY, interest groups)
- Standard of living > raw growth
- Cities evolve, decline, adapt across decades
- No "optimal" solution - just trade-offs

**The tagline practically writes itself:**
"SimCity builds cities. Victoria 3 builds economies. TerminalCity builds both - and watches them change for 100 years."

---

### Synthesis: What Games Miss and TerminalCity's Opportunity

#### Pattern Across All Games: Additive Only

**Civilization:** Tech unlocks new units/buildings, old ones stay relevant
**SimCity:** Population unlocks new zones/services, city only grows
**Banished:** Buildings unlock capabilities, all remain useful

**None of them model:**
- Obsolescence (old things becoming useless)
- Disruption (new things killing old industries)
- Stranded assets (investments that lose value)
- Unemployment (workers displaced by change)
- Decline (cities shrinking, adapting, or failing)
- Transition costs (pain of adapting to change)

#### What TerminalCity Can Do Differently:

**From Civ - Take the Tech Tree Structure:**
- Clear progression with prerequisites
- Unlocks that feel meaningful
- Strategic choices about which path to pursue
- Historical grounding and education value

**From Civ - Add What's Missing:**
- **Disruption:** New tech makes old buildings less viable or more expensive
- **Obsolescence:** Bronze weapons become obsolete when iron arrives
- **Geographic Impact:** Railroad cities boom, canal cities decline
- **Unemployment:** Factory automation displaces workers who must retrain or leave

**From SimCity - Take the Urban Management:**
- Service coverage mechanics
- Traffic and pollution feedback
- Zoning and density systems
- Budget management and trade-offs

**From SimCity - Add What's Missing:**
- **Decline Mechanics:** Cities can shrink, not just grow
- **Suburban Sprawl:** Low-density zones have high infrastructure costs per capita
- **Downtown Death:** Highways and malls kill main street retail
- **Stranded Assets:** Investing in coal plants hurts when switching to solar
- **Economic Transitions:** Industrial cities must adapt or decline

**From Banished - Take the Survival Elements:**
- Infrastructure degradation (tools, roads wear out)
- Seasonal challenges (winter damage)
- Resource loop interdependencies
- Fragility (close to failure creates tension)
- Demographic management (age, education matter)

**From Banished - Add What's Missing:**
- **Tech Progression:** Advance through eras (medieval → industrial → modern)
- **Economic Disruption:** New industries replace old ones
- **External Forces:** Not just starvation - also economic crashes, policy changes, competition
- **Scale:** From village survival to urban management

#### TerminalCity's Unique Proposition:

**Disruption + Adaptation Across 100+ Years:**
- Start in 1955 (or earlier scenario), play through decades of change
- Each tech advance brings benefits AND problems
- Old economic models become obsolete, forcing adaptation
- Cities can decline, go bankrupt, recover, or stabilize at lower level
- Player manages transitions, not just growth
- Victory is surviving and adapting, not just optimizing

**No Other Game Does This:**
- Civ: Tech is pure rewards ladder
- SimCity: Cities only grow or fail
- Banished: No tech progression or disruption
- **TerminalCity: The city builder about change, disruption, and adaptation**

---

## Zoning as Disruption: Mixed-Use vs. Pure Separation

### The Problem with City Builder Zoning

Most city builders (SimCity, Cities: Skylines) model **post-WWII North American zoning** - strict separation of residential, commercial, and industrial into pure zones. But this approach is:
- **Historically recent** (post-1920s)
- **Geographically unusual** (mostly US/Canada suburban pattern)
- **A major disruption** that killed walkable neighborhoods and created car dependency

Real cities, historically and globally, default to **mixed-use development**. Separating everything was a radical change that reshaped urban life.

### A Brief History of Zoning

#### Pre-1900s: No Zoning, Organic Mixed-Use

**How It Worked:**
- Buildings mixed uses naturally based on economics
- Ground floor: Shop, workshop, tavern, stable
- Upper floors: Owner's residence, worker housing, storage
- Everything within walking distance by necessity (no cars)

**Examples:**
- Blacksmith shop with family living above
- General store with apartments on upper floors
- Tavern with inn rooms upstairs
- Bakery with baker's home attached
- Mill with miller's house adjacent

**Why It Worked:**
- Walkable neighborhoods (everything local)
- Live-work arrangements common
- Short commutes (work downstairs)
- Efficient land use
- Street life and community

**Problems:**
- Industrial pollution near housing (no separation)
- Fire risk (dense wooden buildings)
- No environmental controls
- Slaughterhouses, tanneries, factories mixed with homes

#### 1900s-1920s: Early Zoning, Nuisance Control

**The Trigger: Industrial Revolution Problems**
- Factories, slaughterhouses, chemical plants creating serious health hazards
- Tenement overcrowding in industrial cities
- No building codes or safety standards
- Wealthy neighborhoods wanted to exclude "undesirable" uses

**New York City Zoning Resolution (1916):**
- First comprehensive US zoning law
- Created use districts (residential, commercial, industrial)
- Setback requirements (wedding cake buildings)
- Height limits by district
- **Goal:** Separate noxious industry from housing, control building density

**Early Zoning Philosophy:**
- Protect property values
- Separate incompatible uses (factories vs. homes)
- Control building form (height, setbacks)
- But still allowed **mixed-use in commercial districts** (shops + apartments)

#### 1920s-1970s: Euclidean Zoning, Pure Separation

**Euclid v. Ambler (1926) Supreme Court Case:**
- Validated zoning as constitutional
- City of Euclid, Ohio created strict single-use zones
- "Euclidean zoning" becomes the standard across US
- **Radical change:** Separate EVERYTHING

**Post-WWII Zoning (1945-1970s):**
- **Single-family residential zones:** ONLY detached houses, no commercial, no apartments
- **Commercial zones:** ONLY shops/offices, no housing
- **Industrial zones:** ONLY factories/warehouses
- **Minimum lot sizes:** Force low density (1 acre lots)
- **Parking requirements:** Every use needs parking (2-4 spaces per 1000 sq ft)
- **Setback requirements:** Buildings far from street

**The Disruption:**
- **Killed mixed-use neighborhoods:** Can't open shop in residential area
- **Forced car dependency:** Must drive to shop, work, school
- **Created sprawl:** Low density = spread out
- **Killed walkability:** Nothing within walking distance
- **Separated social classes:** Expensive single-family zones exclude apartments
- **Strip malls and big box stores:** Only way to provide retail to sprawling suburbs
- **Downtown decline:** Retail moves to suburban strips with parking

**Why This Happened:**
- **Automobiles enabled it:** Can drive 15 miles to work
- **Post-war housing boom:** Build suburbs fast
- **Racial segregation:** Zoning used to exclude minorities (explicitly or effectively)
- **Property value protection:** Keep "undesirable" uses away
- **Modernist planning ideology:** Separate functions for "efficiency"

**Results:**
- American suburbs: Pure residential, must drive everywhere
- Bedroom communities (TerminalCity's 1955 scenario)
- Highway dependence
- Parking lots everywhere
- Main street retail dies, strip malls thrive
- Social isolation, no street life

#### 1980s-Present: Zoning Reform, Return to Mixed-Use

**New Urbanism Movement:**
- Reaction against car-dependent sprawl
- Advocate for walkable, mixed-use neighborhoods
- "Traditional Neighborhood Development"
- Examples: Seaside FL, Kentlands MD

**Form-Based Codes:**
- Focus on building form, not use
- Allow mixed-use by default
- Control height, setbacks, street relationship
- Don't care if it's shop or apartment, just how it looks

**Transit-Oriented Development (TOD):**
- Mixed-use near transit stations
- Encourage walking to transit
- Reduce parking requirements
- Higher density

**The Challenge:**
- Fighting 70+ years of zoning law
- NIMBYs oppose change ("protect neighborhood character")
- Parking requirements force car-oriented design
- Single-family zoning politically protected
- Slow, incremental reform in some cities

**Examples of Reform:**
- Minneapolis: Eliminated single-family zoning (2018)
- California: Allowing ADUs (accessory dwelling units)
- Oregon: Requiring duplex/triplex in single-family zones
- Some cities: Reduced parking requirements
- But most suburbs: Still pure Euclidean zoning

### Geographic Differences in Zoning

City builders model **one system** (US post-war), but real world has **many systems**:

#### United States: Euclidean Zoning (Exclusive)

**How It Works:**
- Mark zones: R-1 (single-family), R-2 (duplex), C-1 (commercial), I-1 (industrial)
- Each zone allows ONLY specific uses
- Cannot mix uses (shop in residential zone = illegal)
- Minimum lot sizes, parking requirements, setbacks

**Results:**
- Strict separation of uses
- Car dependency required
- Sprawling development
- Difficult to create walkable neighborhoods

**Why US Does This:**
- Historical: Post-war planning ideology
- Political: Property owners want "protection" from change
- Economic: Developers follow established patterns
- Legal: Changing zoning is difficult and expensive

#### Japan: Cumulative Zoning (Hierarchical)

**How It Works:**
- 12 zone categories from most restrictive → most permissive
- Each zone **includes all uses from less intense zones** (cumulative)
- **Not exclusive** - higher zones allow lower-intensity uses

**The 12 Categories (Simplified):**
1. **Category 1 - Exclusive Low-Rise Residential:**
   - Single-family homes, small apartments
   - No shops (most restrictive)

2. **Category 2 - Low-Rise Residential:**
   - Category 1 uses + small shops (150 sq m)
   - Corner stores, small restaurants

3. **Category 3 - Medium-Rise Residential:**
   - Categories 1-2 + larger shops, schools, hospitals
   - Small commercial mixed with residential

4. **Category 4 - Low-Rise Commercial/Residential:**
   - Categories 1-3 + offices, hotels, pachinko parlors
   - Mixed-use neighborhoods

5-6. **Medium-rise and high-rise commercial/residential**

7-10. **Industrial zones** (light → heavy industry)

11-12. **Exclusive industrial** (heavy, noxious industry)

**Key Difference from US:**
- Zone 4 **allows** all uses from zones 1-3 (homes, apartments, shops, offices)
- US system: Each zone allows ONLY its designated use
- **Result:** Mixed-use is default, not exception

**Why This Works:**
- Small shops naturally appear in residential areas (walkable)
- No need to drive for basic errands
- Dense, efficient land use
- Neighborhood shops (konbini, ramen, pharmacy) everywhere
- Less car dependency (though Japan still has cars)

**Cultural Context:**
- High land prices favor density
- Strong public transit reduces car need
- Cultural acceptance of small spaces
- Neighborhood shops preferred over big box stores

#### Europe: Varied Systems, Generally Permissive

**Common Patterns:**
- Mixed-use is traditional (pre-dates cars)
- Historic city centers protected (no demolition for parking)
- Residential + commercial on same street is normal
- Ground floor commercial, upper floor residential is standard

**Examples:**

**Paris:**
- Haussmann buildings: Shops on ground floor, apartments above (standard)
- No pure residential zones in city center
- Walkable by design (pre-car street layouts)

**Barcelona:**
- Mixed-use blocks (residential + commercial + small industry)
- Superblocks (limit through-traffic, prioritize pedestrians)
- Ground floor activation (shops, cafes facing street)

**German Cities:**
- Use-based zoning but allows mixing within zones
- "Mischgebiete" (mixed area) is a formal zone type
- Residential + light commercial encouraged
- Strict rules on building form, permissive on use

**Why Europe Kept Mixed-Use:**
- Cities pre-date zoning laws (already mixed)
- Never fully suburbanized (expensive to rebuild)
- Strong pedestrian culture
- Limited land, high density required
- Public transit reduces car dependency

#### Other Systems:

**UK:**
- "Town and Country Planning" system
- Focuses on individual applications, not zoning maps
- Mixed-use requires approval but generally encouraged
- Conservation areas protect historic character

**Australia:**
- Similar to US but less strict
- Mixed-use encouraged in urban areas
- Suburban areas still car-oriented

### Mixed-Use Buildings: How They Actually Work

#### The Classic Mixed-Use Building

**Structure:**
- **Ground floor:** Retail, restaurant, cafe, office, services
  - Large windows, direct street access
  - Storefront visibility important
  - Higher rent per sq ft (commercial rates)

- **Upper floors:** Apartments, offices, hotel rooms
  - Residential windows (smaller)
  - Separate entrance (side/rear)
  - Lower rent per sq ft than ground floor commercial
  - 2-6 stories typical

**Why This Form:**
- **Economic:** Ground floor most valuable (street visibility)
- **Practical:** Separate residential entrance (don't walk through shop)
- **Urban:** Creates active street life (shops facing sidewalk)
- **Efficient:** Maximizes land use (same footprint, multiple uses)

**Historical Examples:**
- Medieval European cities: Workshop below, craftsman above
- 1800s US cities: Store below, owner's apartment above
- Early 1900s: Storefront + walk-up apartments
- Modern: Retail podium with residential tower above

#### Benefits of Mixed-Use

**For Residents:**
- Walk to shops, cafes, services (no car needed)
- Street life and activity (feels safe, interesting)
- Convenient (errands on ground floor)
- Urban lifestyle (living above the action)

**For Businesses:**
- Built-in customer base (residents above)
- Lower rent than pure commercial district
- Neighborhood regulars (repeat customers)
- Synergies (cafe + bookstore + laundromat = destination)

**For City:**
- Efficient land use (more tax revenue per acre)
- Walkable neighborhoods (less traffic)
- Street activation (eyes on street, safer)
- Less parking needed (walk to errands)
- Public transit viable (density supports it)

**For Urban Vitality:**
- Jane Jacobs' "eyes on the street" (people always around)
- 18-hour neighborhoods (morning coffee, evening dinner, nighttime residents)
- Social mixing (various income levels, ages, uses)
- Pedestrian activity creates sense of place

#### Problems with Pure Zoning

**Residential-Only Zones:**
- Must drive to shop, eat, work
- Empty during workday (everyone commutes out)
- No street life, no walkability
- Requires car ownership (expensive for poor)
- Isolated, boring

**Commercial-Only Zones:**
- Dead at night (offices close, everyone leaves)
- Unsafe (empty streets after 6pm)
- Parking lots dominate (stores far apart)
- Strip malls (drive from store to store)

**The Irony:**
- Zoning meant to improve quality of life
- Created car dependency, isolation, sprawl
- Killed the walkable neighborhoods people love
- Now cities trying to recreate what they destroyed

### Zoning in TerminalCity: Gameplay Implications

#### Era-Based Zoning Systems

The player's available zoning tools change with time period:

**Pre-1920: No Zoning (Organic Development)**
- Buildings place themselves based on economics
- Mixed-use emerges naturally
- Player has minimal control over use (can't force separation)
- Shops appear where demand exists (near housing)
- Workshops cluster near resources/transport
- Everything walkable by necessity

**1920s-1930s: Early Zoning (Nuisance Control)**
- Player can designate zones: Residential, Commercial, Industrial
- **Goal:** Separate noxious industry from housing
- But mixed-use still allowed in commercial zones
- Can zone "residential + compatible commercial"
- Less strict than later eras

**1940s-1970s: Euclidean Zoning (Pure Separation)**
- **Available:** Pure single-use zones
  - R-1 (single-family only, no apartments)
  - R-2 (low-density residential)
  - C-1 (commercial only, no housing)
  - I-1 (industrial)
- **Forces:** Strict separation, minimum lot sizes, parking requirements
- **Result:** Sprawl, car dependency, bedroom communities
- **Player choice:** Do you implement strict zoning or resist?
  - Strict zoning prevents "incompatible" uses, protects property values
  - But kills walkability, forces car ownership, creates sprawl

**1980s+: Zoning Reform (Mixed-Use Returns)**
- **New options:** Mixed-use zones, TOD (Transit-Oriented Development), form-based codes
- **Overlay zones:** Allow mixed-use in specific areas (downtown, transit corridors)
- **Requires:** Fighting existing zoning (political cost)
- **Player choice:** Reform old zoning to allow mixed-use, or keep pure zones
  - Reform enables walkability, but neighbors may oppose
  - Keep pure zoning maintains status quo but limits urban options

#### Zoning as Player Choice with Consequences

**Implement Strict Zoning (1920s+):**
- **Benefits:**
  - "Protect" residential property values
  - Separate industry pollution from homes
  - Predictable development (zone for X, get X)
  - Appeals to wealthy homeowners (political support)

- **Costs:**
  - Kills walkable neighborhoods
  - Forces car dependency (everyone must drive)
  - Sprawling development (requires more roads, sewers, utilities per capita)
  - Downtown retail declines (malls and strips take over)
  - Social segregation (expensive single-family zones exclude poor)
  - Higher infrastructure costs (spread out = expensive)

**Allow Mixed-Use:**
- **Benefits:**
  - Walkable neighborhoods (shops within walking distance)
  - Efficient land use (more tax revenue per acre)
  - Less car dependency (reduces traffic, parking demand)
  - Supports public transit (density makes it viable)
  - Street life and community

- **Costs:**
  - Property owners may complain (oppose change)
  - "Neighborhood character" arguments (NIMBYs)
  - Less predictable (mix of uses)
  - May reduce property values in exclusive areas (no longer exclusive)

**Player Strategy:**
- Early game (1800s-1910s): Mixed-use by default, can't prevent it
- Mid game (1920s-1970s): Choice to implement zoning or not
  - Strict zoning = suburban sprawl model (cheaper upfront, expensive long-term)
  - Mixed-use = urban density model (requires planning, more efficient)
- Late game (1980s+): Can reform zoning if you went pure separation
  - Upzone near transit for TOD
  - Allow ADUs in single-family zones
  - Create mixed-use overlay districts

#### Visual Representation

**Mixed-Use Building at Different Zooms:**

**25ft Zoom (Closest):**
See distinct floors and uses:
```
╔═══╗ ← Apartments (residential color, e.g., white/beige)
║:::║ ← Apartments
╠═══╣ ← Transition
║▓▓▓║ ← Shop (commercial color, e.g., blue/gold)
╚═══╝
```

**50ft Zoom:**
Still show it's mixed-use:
```
⌂ (but with split colors: top half residential, bottom commercial)
```

**100ft Zoom:**
Single glyph, but marked as mixed-use in data:
```
⌂ (standard building glyph)
```

**Hover/Inspect:**
- Shows: "Mixed-Use Building"
- Ground floor: "Corner Store"
- Floors 2-3: "4 Apartments"
- Residents: 8
- Jobs: 2 (shop workers)

**Building Decay Representation:**

**Well-Maintained Mixed-Use:**
```
╔═══╗  (bright colors, clean lines)
║:::║
╠═══╣
║▓▓▓║
╚═══╝
```

**Declining:**
```
╔═╤═╗  (colors fade, some characters change)
║:░:║  (░ shows deterioration)
╠═╧═╣
║▒▓▒║  (▒ shows neglect)
╚═══╝
```

**Abandoned:**
```
╔░╤▒╗  (heavy character corruption, dark colors)
░:░░║  (building barely recognizable)
╠▓╧░╣
▒░▒▓░  (random characters showing decay)
╚▒░═╝
```

#### Adaptive Reuse Mechanic

**Old Industrial Building → Mixed-Use Residential/Commercial:**

1. **Abandoned Factory:**
   - Industrial zone no longer viable (jobs left)
   - Building sits empty, decaying
   - Visual: Industrial glyph (▓▓▓) with decay (▒░▓░▒)

2. **Player Decision: Rezone + Renovate:**
   - Change zoning from industrial to mixed-use
   - Requires investment ($$$) to gut and renovate
   - Time: 1-2 years of construction
   - Cheaper than demolish + rebuild (shell exists)

3. **Renovated Loft Building:**
   - Ground floor: Restaurants, galleries, shops (trendy)
   - Upper floors: Loft apartments (high ceilings, industrial feel)
   - Appeals to: Young professionals, artists, creatives
   - Visual: Mixed-use glyph with bright colors (renovated)
   - Higher tax revenue than abandoned factory
   - Neighborhood revitalization (catalyst for more investment)

**Real-World Examples:**
- Brooklyn Navy Yard → Creative offices + light manufacturing
- Detroit's Packard Plant → (some attempts at reuse)
- Minneapolis Mill District → Loft apartments + restaurants
- Portland's Pearl District → Warehouses → Mixed-use

**Gameplay:**
- Player can trigger adaptive reuse projects
- Costs money but revitalizes declining areas
- Risk: Gentrification (displaces existing residents)
- Reward: Tax revenue increases, neighborhood quality improves

#### Agricultural Transition: Family Farms → Commercial Farms

**1800s-1950s: Family Farms (Small-Scale):**
- Plot size: 40-160 acres
- Labor: Family members, some hired hands
- Equipment: Horses, basic tools
- Crops: Diverse (wheat, corn, vegetables, chickens, cows)
- Productivity: Low yield per acre
- Economics: Self-sufficiency + sell surplus
- Visual: Many small farmsteads, each with house + barn + fields
- Population: Many families per square mile

**1950s+: Commercial Farms (Industrial-Scale):**
- Plot size: 1,000-10,000+ acres
- Labor: Few workers, mostly machine operators
- Equipment: Tractors, combines, industrial machinery
- Crops: Monoculture (corn, soybeans, wheat)
- Productivity: High yield per acre
- Economics: Commodity production for global market
- Visual: Few large operations, minimal buildings
- Population: Few families per square mile

**The Disruption:**

**What Happens:**
- Small farms can't compete economically (economies of scale)
- Farm kids move to cities (only one inherits farm, others leave)
- Neighbor farms consolidated (buy out family farms)
- Rural towns depopulate (fewer people to support businesses)
- Equipment dealers replace general stores
- Grain elevators centralized
- Schools close (not enough children)

**Visual Change Over Time:**
```
1950 Map View:
⌂ ⌂ ⌂   (many small farmsteads)
⌂ ⌂ ⌂   (each with family)
⌂ ⌂ ⌂

2000 Map View:
. . .   (fields with no buildings)
. ⌂ .   (one large operation)
. . .   (most farmsteads gone)
```

**Player Decisions:**
- Do you zone for agricultural consolidation? (efficient but depopulates)
- Do you try to preserve small farms? (subsidies, zoning that limits consolidation)
- How do you handle dying rural towns? (where farm families used to shop)

**Consequences:**
- Efficient agriculture = cheap food, but rural decline
- Small-town Main Streets die (not enough customers)
- Rural schools close (consolidate into regional schools)
- Loss of rural culture and community
- Environmental: Monoculture → pesticides, soil depletion, biodiversity loss

#### Political Dimension: NIMBYs vs. YIMBYs

Zoning changes don't happen in a vacuum - they're intensely political. Different interest groups fight over every rezoning decision.

**NIMBY: "Not In My Back Yard"**

**Who They Are:**
- Existing homeowners (especially single-family)
- Older residents (established in neighborhood)
- Wealthy property owners
- People who benefit from exclusionary zoning

**What They Oppose:**
- Apartments/multifamily housing ("too dense")
- Affordable housing ("lowers property values")
- Mixed-use development ("traffic and parking")
- Transit stations ("attracts crime")
- Homeless shelters, group homes, etc.
- Any change to "neighborhood character"

**Their Arguments:**
- "Protect property values"
- "Too much traffic"
- "Not enough parking"
- "Shadows on my yard"
- "School overcrowding"
- "Preserve neighborhood character"
- "Think of the children"
- Often coded language for class/race exclusion

**Their Power:**
- Homeowners vote consistently (high turnout)
- Attend public hearings (retired, have time)
- Hire lawyers to challenge projects
- Organize neighborhood groups
- Political donations to local officials
- Can delay/kill projects for years

**Real-World Impact:**
- San Francisco: NIMBY stronghold, almost nothing gets built, housing crisis
- Berkeley: Years of battles over every small apartment building
- Wealthy suburbs: Minimum lot sizes, no apartments allowed
- California: State had to override local NIMBY control (SB 9, SB 10)

**YIMBY: "Yes In My Back Yard"**

**Who They Are:**
- Renters (locked out of homeownership)
- Young people (can't afford housing)
- Urbanists (want walkable neighborhoods)
- Climate activists (density reduces emissions)
- Affordable housing advocates
- Some homeowners (minority)

**What They Support:**
- New housing (especially apartments)
- Upzoning (allow more density)
- Mixed-use development (walkability)
- Transit-oriented development
- Affordable housing mandates
- Legalizing ADUs (accessory dwelling units)
- Eliminating single-family-only zoning

**Their Arguments:**
- "Housing shortage crisis"
- "Let people build homes"
- "Walkable neighborhoods"
- "Climate change (density > sprawl)"
- "Economic opportunity"
- "Anti-exclusion/segregation"
- "Vibrant urban life"

**Their Power:**
- Growing movement (especially in expensive cities)
- Social media organizing
- State-level policy changes (override local NIMBYs)
- Some electoral victories (city councils)
- Coalition with climate/progressive groups
- But: Renters vote less, have less money than homeowner NIMBYs

**Real-World Victories:**
- Minneapolis: Eliminated single-family zoning citywide (2018)
- Oregon: Required duplex/fourplex in all single-family zones (2019)
- California: Legalized ADUs statewide, limited local control
- Austin: Allowing more density near transit
- New Zealand: Allowed 3-story townhouses nationwide (2020)

**The Political Battle:**

Most zoning fights follow this pattern:

1. **Developer proposes project:** 6-story apartment building in single-family area
2. **Planning department recommends approval:** Meets zoning goals, housing shortage
3. **NIMBYs organize:** Neighborhood association opposes, packs public hearing
4. **Arguments fly:**
   - NIMBYs: "Too tall! Too dense! Traffic! Parking! Character!"
   - YIMBYs: "Housing crisis! Walkability! Climate! Let people live here!"
5. **City council decides:**
   - Deny (NIMBY victory) → Developer appeals or gives up
   - Approve (YIMBY victory) → NIMBYs sue, delay for years
   - Compromise (reduce from 6 stories to 4) → Everyone unhappy

**Why NIMBYs Usually Win:**
- Homeowners have more time, money, political power
- Status quo bias (easier to stop change than make change)
- "Neighborhood input" gives veto power to loudest voices
- Local officials fear homeowner backlash
- Takes years of litigation to win

**Why YIMBYs Sometimes Win Now:**
- Housing crisis too severe to ignore
- State/federal override of local control
- Generational shift (young people desperate for housing)
- Climate crisis makes sprawl untenable
- Courts striking down exclusionary zoning (in some states)

#### NIMBY/YIMBY Gameplay Mechanics

**When Player Tries to Rezone:**

Example: Change single-family zone to allow apartments

**Step 1: Proposal**
- Player selects area, proposes rezoning
- Game calculates affected residents and their stance

**Step 2: Opposition Meter**
Shows political breakdown:
```
NIMBY Opposition: ████████░░ 80%
YIMBY Support:    ███░░░░░░░ 30%
Neutral:          ██░░░░░░░░ 20%

Likelihood of Approval: Low
Expected Delay: 3-5 years of hearings
Political Cost: -25 approval with homeowners, +15 approval with renters
```

**Step 3: Political Actions**

Player can take actions to shift the balance:

**Build YIMBY Coalition:**
- Host community meetings (+10% YIMBY support)
- Partner with housing advocates (+15% support)
- Cite housing shortage data (+5% support)
- Cost: Time and political capital

**Compromise with NIMBYs:**
- Reduce height (6 stories → 4 stories)
- Reduce density (100 units → 60 units)
- Add parking (even though it's wasteful)
- "Preserve character" (restrict design)
- Result: Less opposition, but worse project

**Force Through (High Risk):**
- Use state preemption law (override local NIMBYs)
- Majority vote on city council
- Result: Project approved, but:
  - -40 approval rating with homeowners
  - Risk of recall election
  - Future projects face more opposition
  - May lose next election

**Give Up:**
- Cancel rezoning proposal
- Status quo remains
- No political cost
- But housing shortage worsens

**Step 4: Outcomes**

**NIMBY Victory:**
- Rezoning denied or watered down to uselessness
- Neighborhood stays single-family
- Housing shortage continues
- Property values increase (scarcity)
- NIMBYs emboldened for next fight

**YIMBY Victory:**
- Rezoning approved (maybe after years)
- Apartments get built
- More housing supply (slight rent decrease)
- NIMBYs angry, organize for next election
- Neighborhood becomes more walkable/diverse

**Compromise:**
- Some density allowed, not all requested
- Both sides unhappy
- Incremental progress
- Opens door for future upzoning

**Time Factor:**
- NIMBY opposition can delay projects 2-10 years
- During delay:
  - Housing shortage worsens
  - Construction costs increase
  - Developer may give up
  - Political landscape may shift

**Long-Term Dynamics:**

**NIMBY Dominance (1950-2010):**
- Almost nothing gets built in established neighborhoods
- All growth happens on fringe (sprawl)
- Housing becomes unaffordable
- Young people leave city (can't afford it)
- Economic stagnation (workers can't find housing)

**YIMBY Momentum (2010-present in some cities):**
- State laws override local NIMBYs
- More apartments get built
- Rents stabilize (increased supply)
- More walkable neighborhoods
- But: Gentrification concerns (who benefits?)

**Player Strategy:**

**Early Game (1920s-1970s):**
- Little organized NIMBY opposition yet
- Can implement zoning without much pushback
- But once established, hard to change

**Mid Game (1970s-2000s):**
- NIMBY power peaks
- Very difficult to upzone
- Most new development happens on fringe
- Suburban sprawl model dominates

**Late Game (2010+):**
- YIMBY movement emerges
- State preemption possible
- Can reform old zoning, but political battles intense
- Player must navigate NIMBY/YIMBY conflict

**Visual Representation:**

**When hovering over proposed rezoning:**
```
┌─────────────────────────────────┐
│ Proposed: Rezone to Mixed-Use   │
│                                  │
│ Political Opposition:            │
│ ████████░░ 80% NIMBY            │
│ ███░░░░░░░ 30% YIMBY            │
│                                  │
│ Top Concerns:                    │
│ • Traffic congestion             │
│ • Parking shortage               │
│ • "Neighborhood character"       │
│                                  │
│ Approval: Low (28%)              │
│ Time: 4-6 years if opposed       │
│ Political Cost: -30 homeowners   │
│                                  │
│ [Build Coalition] [Compromise]   │
│ [Force Vote] [Cancel]            │
└─────────────────────────────────┘
```

**During Public Hearing (Event):**
```
PUBLIC HEARING: Mixed-Use Rezoning

NIMBY Speakers (12):
"This will destroy our neighborhood!"
"Traffic is already terrible!"
"Where will people park?!"
"Property values will plummet!"

YIMBY Speakers (3):
"We have a housing crisis!"
"Young people can't afford to live here!"
"This is about basic fairness!"

City Council leans: 4 No, 3 Yes, 2 Undecided

Your action:
[Speak in favor] [Offer compromise] [Withdraw proposal]
```

**Electoral Consequences:**

If you force through unpopular zoning changes:
```
ELECTION RESULTS

Your approval rating: 42% (was 65%)

Homeowner bloc: Organized opposition
"Vote out the council that ruined our neighborhoods!"

NIMBY-backed candidate wins: 54% - 46%

You are voted out of office.

Continue as new mayor? [Yes] [No]
```

**The Meta-Game:**
- Player must balance housing needs with political survival
- Sometimes the right policy (more housing) is politically costly
- Can you build a YIMBY coalition strong enough to win?
- Or do you give in to NIMBYs and watch housing crisis worsen?
- No easy answers - just trade-offs and consequences

### Design Philosophy: Zoning Shapes Cities

**Key Insights:**

1. **Zoning is not neutral:** It's a powerful tool that shapes how cities develop, who can live where, and how people move through space

2. **Zoning is historically recent:** Pre-1920s cities had no zoning - mixed-use was default

3. **Zoning is culturally specific:** US suburban model is unusual globally - most cities mix uses

4. **Zoning laws were a disruption:** Killed walkable neighborhoods, created car dependency, separated social classes

5. **Zoning can be reformed:** Some cities bringing back mixed-use, but it's politically difficult

**For TerminalCity:**

- **Don't just model one zoning system** (SimCity's pure zones)
- **Model the evolution:** No zoning → Nuisance control → Pure separation → Reform attempts
- **Give player agency:** Choose to implement strict zoning or allow mixed-use
- **Show consequences:** Strict zoning = sprawl + car dependency, Mixed-use = walkability + density
- **Geographic variety:** Different countries/regions zone differently
- **Time period matters:** What's possible in 1880 vs. 1950 vs. 2000 is different

**The Most Interesting Gameplay:**
- Not "build the optimal city"
- But "navigate changing zoning paradigms across 100 years"
- Do you embrace car-oriented sprawl in 1955?
- Do you try to preserve walkable neighborhoods?
- Do you fight to reform zoning in 1990s?
- How do you handle decline of areas zoned for obsolete uses?

**Zoning is Disruption:**
Just like automobiles, internet, or any technology - zoning laws **changed everything about how cities work**. TerminalCity should model that disruption, not treat zoning as a neutral tool.

---

## Abstraction Level: Policy Not Micromanagement

### Core Design Principle: You're the Mayor, Not the City Manager

The player should operate at the **policy and budget level**, not the **individual task level**. You don't manually patch potholes, route garbage trucks, or arrest criminals - you set budgets and policies, and city departments handle execution.

**Why This Matters:**
- **Scales to large cities:** Can't micromanage 100,000 residents
- **Creates strategic gameplay:** Think about trade-offs, not tedious tasks
- **Feels realistic:** Mayors don't patch potholes, they allocate budgets
- **Allows emergent consequences:** Policies create systemic effects that play out naturally
- **Reduces busywork:** Focus on interesting decisions, not repetitive clicking

### What the Player Controls

**Budget Allocation:**
- Road maintenance: $500k/year
- Police department: $2M/year
- Fire services: $1M/year
- Schools: $5M/year
- Parks & recreation: $300k/year
- Public transit: $3M/year

**Staffing Levels:**
- Hire/fire city employees
- Set department sizes
- Balance service quality vs. cost

**Policy Decisions:**
- Pass ordinances (zoning, labor laws, environmental rules)
- Tax rates (property, sales, business)
- Development priorities (which areas get investment)
- Service coverage (expand to new areas? Abandon declining ones?)

**Capital Projects:**
- Build new infrastructure (roads, bridges, transit lines)
- Build service buildings (fire stations, schools, police stations)
- Urban redevelopment projects

### What Happens Automatically

**Service Execution:**
- Road crews patrol, find potholes, patch them
- Garbage trucks run routes, collect trash
- Police patrol areas, respond to crime
- Fire department responds to fires
- Teachers teach students
- Parks maintenance crews mow lawns, repair equipment

**Consequences Emerge:**
- Well-funded services → Good outcomes (clean streets, low crime, good schools)
- Underfunded services → Bad outcomes (potholes, crime, declining education)
- Visual feedback shows results (decay, graffiti, trash, broken equipment)
- Residents react (complain, move away, vote against you)

**Economic Ripples:**
- City employees are real residents with jobs
- Budget cuts = layoffs = unemployment = reduced spending
- Ripple effects through local economy
- Political backlash from affected workers/unions

### Example: Road Maintenance

**The Wrong Way (Micromanagement):**
❌ Click on each pothole individually to fix it
❌ Manually route maintenance trucks
❌ Choose which roads get repaired today
❌ *Tedious, doesn't scale, misses the point*

**The Right Way (Policy/Budget):**
✅ Set road maintenance budget: $500k/year
✅ Hire 50 public works employees
✅ Game simulates: Crews patrol city, identify issues, repair them
✅ Visual feedback: Roads look good (well-funded) or decay (underfunded)
✅ Consequences emerge: Businesses complain if roads bad, property values drop
✅ *Strategic, scalable, creates interesting trade-offs*

**Budget Crisis Example:**

**Year 1: Adequate Funding**
- Roads budget: $500k
- Public works: 50 employees
- Result: All roads maintained, minimal potholes, roads look pristine

**Year 5: Budget Crisis (Must Cut 30%)**
- Player cuts road budget to $350k
- Game automatically lays off 15 workers (30% reduction)
- Remaining 35 workers can't cover entire city
- Result:
  - Potholes accumulate faster than repairs
  - Visual: Road tiles show decay (character changes, color fades)
  - Residents complain: "Roads are terrible!"
  - Businesses complain: "Delivery trucks getting damaged!"
  - Property values drop in worst-affected areas
  - 15 families lose income (laid-off workers)

**Year 8: Crisis Compounds**
- Roads so bad people and businesses start leaving
- Less tax revenue = deeper budget hole
- Must choose: Raise taxes (political cost)? Cut other services? Issue bonds (debt)?
- **You made one budget decision, system simulated cascading consequences**

**Year 10: Recovery?**
- If you restore funding: Crews gradually catch up, roads improve over 2-3 years
- If you don't: Death spiral continues (decline → less revenue → more cuts → more decline)

### City Services Employ Real People

**Public Sector Employment:**

City services aren't abstract - they employ real residents:

**Public Works Department:**
- 50 employees
- Average salary: $35k/year
- Total payroll: $1.75M/year
- These are residents living in your city
- They pay rent, buy goods, support local businesses

**Police Department:**
- 100 officers
- Average salary: $50k/year
- Total payroll: $5M/year

**Fire Department:**
- 40 firefighters
- Average salary: $45k/year
- Total payroll: $1.8M/year

**Schools:**
- 200 teachers
- Average salary: $40k/year
- Total payroll: $8M/year

**Parks & Recreation:**
- 20 employees
- Average salary: $30k/year
- Total payroll: $600k/year

**Total Public Sector:** 410 jobs, $17.15M payroll

**Budget Cuts = Unemployment Crisis:**

When you cut budgets, you're laying off real people:

1. **Cut 20% across all departments**
2. **82 people lose their jobs**
3. **82 families lose income**
4. **Those families:**
   - Can't pay rent (might move away or become homeless)
   - Stop buying goods (local businesses lose customers)
   - Kids might need to change schools (family stress)
   - Radicalize politically (vote against you, join protests)

5. **Ripple effects:**
   - Local shops lose customers (laid-off workers have no money)
   - Landlords lose tenants (can't pay rent)
   - Other businesses cut staff (reduced demand)
   - Tax revenue drops further (fewer employed people)
   - **Negative spiral**

6. **Political consequences:**
   - Public sector unions organize protests
   - "Save our jobs!" rallies
   - Opposition candidate runs on "restore services"
   - You might lose next election

**This is way more interesting than abstract budget numbers!**

### Visual Feedback: See the Consequences

**Well-Maintained Neighborhood (Adequate Budget):**

**Roads:**
- Clean asphalt appearance
- Bright colors, clear lines
- No potholes or cracks
- Character: ═══ (clean, straight)

**Parks:**
- Mowed grass: . . . (green, uniform)
- Working equipment
- People using facilities
- Benches, fountains maintained

**Buildings:**
- Well-kept appearance
- Fresh paint (bright colors)
- No broken windows
- No graffiti

**Street Infrastructure:**
- Street lights working
- Trash bins emptied
- Clean sidewalks

**Overall Feel:** Prosperous, cared-for, attractive

---

**Neglected Neighborhood (Budget Cuts):**

**Roads:**
- Potholes visible
- Character decay: ═══ → ▓░▓ (broken, patchy)
- Colors fade (gray instead of black)
- Cracks, deterioration obvious

**Parks:**
- Overgrown grass: . . . → ░░░ (weedy, unkempt)
- Broken equipment (no repairs)
- Empty (people avoid it)
- Graffiti on facilities

**Buildings:**
- Decay setting in
- Peeling paint (faded colors)
- Broken windows (█ → ▓)
- Graffiti appears (random color splotches)

**Street Infrastructure:**
- Street lights out (not replaced)
- Trash piling up: ☼ symbols appear
- Dirty sidewalks

**Overall Feel:** Declining, neglected, dangerous

**Player learns immediately:** "My budget cuts have real visual consequences"

### Service Coverage and Prioritization

During budget crisis, player must choose: **Which areas get services?**

**Option 1: Spread Thin (Equal but Mediocre)**
- Cover entire city with reduced service
- Everywhere gets some maintenance, but all areas decline
- Political: No one feels singled out, but everyone unhappy
- Result: Uniform mediocrity

**Option 2: Triage (Prioritize Core)**
- Focus resources on downtown + viable neighborhoods
- Let outer neighborhoods decline (no services)
- Political: Outer neighborhoods revolt, inner city satisfied
- Result: Patchwork city (thriving core, abandoned periphery)
- **This is the Detroit model!**

**Option 3: Geographic Abandonment**
- Officially declare some neighborhoods "no longer serviced"
- Encourage residents to relocate to core
- Save money by not serving abandoned areas
- Political: Controversial, but honest
- Result: Managed shrinkage

**Each strategy has different consequences - no "correct" answer**

### Other Services That Work This Way

**Police Department:**

**Player Controls:**
- Police budget: $2M/year
- Number of officers: 100
- Station locations (build/close stations)

**Game Handles:**
- Patrol routes automatically
- Crime response automatic
- Coverage areas calculated by station proximity

**Consequences:**
- Well-funded + good coverage: Low crime, fast response, residents feel safe
- Underfunded or poor coverage: High crime, slow response, residents complain
- Visual: High crime areas show graffiti, broken windows, fewer pedestrians

**Budget Cut Impact:**
- Cut 30% → Lay off 30 officers → 70 officers can't cover same area
- Response times increase (fires might spread before police arrive to manage crowd)
- Crime increases in under-covered areas
- Property values drop
- Businesses close (unsafe for customers)
- Residents move away (don't feel safe)

---

**Fire Department:**

**Player Controls:**
- Fire department budget: $1M/year
- Number of firefighters: 40
- Station locations

**Game Handles:**
- Fire response automatic (when fire starts, nearest station responds)
- Response time based on distance to station
- Fire spread based on response time

**Consequences:**
- Good coverage: Fires contained quickly, minimal damage
- Poor coverage: Fires spread, buildings burn down, lives lost
- Visual: Burned-out buildings (black, charred appearance)

**Budget Cut Impact:**
- Close station → Increase response time for that area
- Slower response → Fires spread more before contained
- Buildings burn down → Property loss, displacement
- Insurance rates increase (high fire risk)
- Residents/businesses avoid high-risk areas

---

**Garbage Collection:**

**Player Controls:**
- Sanitation budget: $800k/year
- Number of collectors: 60
- Collection frequency (daily, weekly)

**Game Handles:**
- Routes automatically covered
- Trash accumulates at buildings
- Collection crews empty trash on schedule

**Consequences:**
- Good service: Clean streets, no visible trash
- Poor service: Trash piles up, rats appear, disease risk increases
- Visual: Trash pile symbols (☼) appear on undercollected streets

**Budget Cut Impact:**
- Reduce frequency: Weekly → bi-weekly
- Trash accumulates between collections
- Visual filth increases
- Disease outbreaks (health crisis)
- Residents complain about smell, rats
- Property values drop

---

**Schools:**

**Player Controls:**
- Education budget: $5M/year
- Number of teachers: 200
- Schools (build/close, set capacity)

**Game Handles:**
- Class sizes calculated (students / teachers)
- Education quality based on funding per student
- Students automatically attend nearest school

**Consequences:**
- Well-funded: Small classes (15-20 students), good education, high test scores
- Underfunded: Large classes (35+ students), poor education, low test scores
- Quality affects: Future workforce skill level, families' location choices

**Budget Cut Impact:**
- Lay off 50 teachers → Remaining 150 teachers serve same students
- Class sizes increase: 20 → 30 students per class
- Education quality drops
- Test scores fall
- Families with children move to cities with better schools
- Long-term: Workforce less educated, city less competitive

---

**Parks & Recreation:**

**Player Controls:**
- Parks budget: $300k/year
- Park maintenance crews: 20
- Build new parks or close existing

**Game Handles:**
- Grass mowing, equipment repair, facility cleaning
- Park usage based on maintenance quality

**Consequences:**
- Well-maintained: Beautiful parks, active usage, property value boost nearby
- Neglected: Overgrown, broken equipment, graffiti, empty, property value drop
- Visual: Park tiles show decay (green grass → brown weeds, equipment broken)

**Budget Cut Impact:**
- Reduce crews: 20 → 12
- Parks become overgrown and run-down
- Visual decay obvious (weeds, graffiti, broken benches)
- People stop using parks (feel unsafe, unpleasant)
- Nearby property values drop (view of run-down park)
- Quality of life decreases

### The Philosophy: Systems Over Tasks

**Bad Design (SimCity 1989 style):**
- Click on fire, manually dispatch truck
- Click on criminal, manually send cop
- Place each individual tree in park
- *Tedious micromanagement, doesn't scale*

**Good Design (Policy/Systems):**
- Set fire department budget, build stations
- Fires handled automatically based on coverage
- See visual results: Fast response = contained, slow = building burns
- *Strategic decisions with emergent consequences*

**Great Design (TerminalCity):**
- Set budget, see visual results (decay or prosperity)
- Budget cuts have human cost (layoffs, unemployment)
- Ripple effects through economy (laid-off workers can't spend)
- Political consequences (unions protest, vote against you)
- Long-term compounding (death spirals or virtuous cycles)
- *Systemic, consequential, creates stories*

### Integration with Other Systems

**This abstraction level works with:**

**Victoria 3-style Pops:**
- City workers are real Pops with jobs
- Layoffs = unemployment = radicalization
- Public sector unions become interest group

**NIMBY/YIMBY Politics:**
- Service cuts → Residents angry → Vote against you
- Must balance service quality vs. taxes

**Economic Disruption:**
- City workers lose jobs → Local businesses suffer
- Can't maintain infrastructure during decline
- Visual decay accelerates

**Bankruptcy and Decline:**
- Budget crisis forces service cuts
- Service cuts accelerate decline
- Player must triage: What to save? What to abandon?

### Design Principle Summary

**Player should:**
- Set budgets and policies (macro)
- Make strategic trade-offs (police vs. schools vs. roads)
- See consequences emerge naturally
- Respond to crises with policy changes
- Think about long-term implications

**Player should NOT:**
- Click on individual potholes
- Manually route service vehicles
- Place each trash bin or street light
- Micromanage individual workers
- Do repetitive tedious tasks

**The game should:**
- Simulate services automatically
- Show visual feedback of service quality
- Model employment effects (city workers are real people)
- Create ripple effects (layoffs → economic decline)
- Generate political reactions (cuts → protests)
- Make consequences feel meaningful

**Result:**
- Strategic gameplay, not busywork
- Scales to large cities
- Creates interesting dilemmas
- Feels realistic (you're the mayor, not doing every job yourself)
- Produces emergent stories ("My budget cuts started a death spiral")

---

## Avoiding the Race to the Bottom: Better But Harder Paths

### Core Philosophy: Better Is Possible, Just More Difficult

The game should present morally uncomfortable choices (bulldoze poor neighborhood for highway), but also show **better alternatives exist**. They're just more expensive, politically harder, or require long-term thinking.

**Don't make the game cynical** ("everything sucks, you'll always do bad things")

**Make it educational** ("here's what went wrong historically, here's what works better, you choose")

**The Challenge:**
- Easy path: Do what US cities did (highways, sprawl, displacement) - cheap, quick, politically easy, morally compromising
- Hard path: Do what better cities did (transit, density, inclusion) - expensive, slow, politically difficult, morally sound
- Player must weigh: Short-term pragmatism vs. long-term vision

### The Highway Dilemma: All Options on the Table

**Scenario:** Federal highway funding available ($50M). City needs better transportation.

**Option A: Highway Through Poor Neighborhood** ❌💰
- **Cost:** $50M (fully funded)
- **Time:** 5 years
- **Displacement:** 2,000 residents (mostly Black/poor families)
- **Political Resistance:** Low (20%) - residents have no power
- **Economic Impact:** Minimal (low property values, few businesses)
- **Long-term:** Segregation, downtown decline, car dependency
- **Historical:** What most US cities did (Detroit I-375, Boston I-93, Cross-Bronx Expressway)
- **Moral:** Terrible - destroys community, perpetuates inequality

**Option B: Highway Through Wealthy Neighborhood** ❌💸💸💸
- **Cost:** $500M (10x higher property values + legal battles)
- **Time:** 15+ years (lawsuits, delays)
- **Displacement:** 500 residents (wealthy homeowners)
- **Political Resistance:** Extreme (85%) - lawyers, politicians, media campaign
- **Economic Impact:** Major (lose factories, offices, high tax revenue)
- **Long-term:** Economic damage
- **Historical:** Almost never happened (Jane Jacobs saved Greenwich Village, rare victory)
- **Moral:** More equitable, but economically destructive and politically impossible

**Option C: Build Subway Instead** ✅💰💰
- **Cost:** $300M (6x highway, but less than Option B)
- **Time:** 10 years
- **Displacement:** 0 (underground)
- **Capacity:** 40,000 people/hour (vs. 8,000 for highway)
- **Political Resistance:** Moderate (40%) - requires vision, patience
- **Economic Impact:** Positive (higher capacity, no destruction, property values increase along line)
- **Long-term:** Dense, walkable, transit-oriented development
- **Historical:** What European/Asian cities did (Paris Metro, Tokyo subway)
- **Challenge:** Need to find $250M more funding (see Funding Mechanisms below)
- **Moral:** Best option - no displacement, better capacity, sustainable

**Option D: Light Rail + Bus Rapid Transit** ✅💰
- **Cost:** $120M (compromise)
- **Time:** 5 years
- **Displacement:** 30 buildings (mostly commercial, some housing)
- **Capacity:** 15,000 people/hour (better than highway, less than subway)
- **Political Resistance:** Moderate (35%)
- **Economic Impact:** Positive (transit-oriented development)
- **Long-term:** Good transit, some walkability
- **Historical:** Portland, Minneapolis, many cities post-1990
- **Challenge:** Need $70M more funding
- **Moral:** Good compromise - minimal harm, decent capacity

**Option E: Congestion Pricing (London Model)** ✅💸
- **Cost:** $10M (tolling infrastructure)
- **Time:** 2 years
- **Displacement:** 0
- **Effect:** Reduce traffic by pricing (charge $15 to drive downtown)
- **Revenue:** $20M/year (use for transit improvements)
- **Political Resistance:** High initially (65%), drops to 30% after implementation
- **Long-term:** Less traffic, funds transit, behavior change
- **Historical:** London (2003), Singapore (1975), Stockholm (2006)
- **Challenge:** Drivers hate it initially (eventual acceptance)
- **Moral:** Good - changes incentives, no harm, revenue-positive

**Option F: Protected Bike Lanes + Bus Network** ✅
- **Cost:** $25M
- **Time:** 3 years
- **Displacement:** 0 (use existing roads)
- **Effect:** Gradual mode shift (bikes + buses absorb some car trips)
- **Political Resistance:** Moderate (45% - drivers complain)
- **Long-term:** Healthier, cheaper, works for short/medium trips
- **Historical:** Netherlands (1970s+), Copenhagen, Barcelona
- **Challenge:** Only works in dense areas, weather-dependent
- **Moral:** Good - minimal cost, healthy, sustainable

**Option G: Do Nothing + Remote Work Policy** ✅
- **Cost:** $5M (fiber optic infrastructure)
- **Time:** 1 year
- **Effect:** Enable remote work (reduces commute demand)
- **Political Resistance:** Low (businesses vary)
- **Long-term:** Less traffic, but might hurt downtown retail
- **Historical:** COVID-19 forced experiment (2020+)
- **Challenge:** Not all jobs can be remote, uneven effects
- **Moral:** Neutral - solves one problem, creates others

**Option H: Reject Funding** ❓
- **Cost:** $0
- **Effect:** Keep neighborhoods intact, traffic stays bad
- **Political Resistance:** Mixed (residents happy, businesses unhappy)
- **Long-term:** Congestion worsens, businesses threaten to leave, neighboring cities get highways
- **Historical:** Rare (San Francisco rejected some freeways)
- **Challenge:** Fall behind competitors economically
- **Moral:** Preserves community but city loses competitiveness

**The Game Presents All Options:**
- Shows cost, time, displacement, political resistance for each
- Explains trade-offs clearly
- References historical examples ("This is what Vienna did")
- Lets player choose based on their values + situation
- **No "correct" answer - just consequences**

### International Examples: How Cities Avoided Bad Outcomes

#### Vienna, Austria: Social Housing as Public Good 🏆

**The Context:**
- Post-WWI (1920s): Housing crisis, workers living in slums
- Socialist city government decides: Housing is a public good, not commodity
- Built "Gemeindebauten" (municipal housing) continuously for 100+ years

**What They Built:**
- **60% of Vienna residents live in social housing** (subsidized rent)
- Not just for poor - **mixed-income by design** (teachers, nurses, janitors, engineers all eligible)
- High-quality architecture (beautiful buildings, not concrete blocks)
- Includes: Courtyards, playgrounds, libraries, community centers, pools
- Well-located (throughout city, not isolated in projects)

**How It's Funded:**
- **Land value tax** (tax on property value, incentivizes development)
- Small federal subsidies
- Rent from existing units helps fund new construction
- Sustainable over 100 years

**Why It Works:**
- **Mixed-income = no stigma** (not "projects for poor people")
- **High quality = desirable** (people want to live there)
- **Below-market rent but well-maintained** (rent ~€10/m² vs. €15-20/m² private)
- **Stable communities** (people stay for decades)
- **No speculation** (government owns, doesn't flip for profit)

**Results:**
- **Most affordable major city in Western Europe**
- **Low homelessness** (everyone can find housing)
- **High quality of life** (stable rents, good housing)
- **Economically diverse neighborhoods** (not segregated by income)

**TerminalCity Application:**

**Player Option: Vienna-Style Social Housing**
- **Cost:** $500M initial investment (build 10,000 units)
- **Ongoing:** $50M/year subsidy (rent doesn't cover full costs)
- **Funding:** Land value tax ($40M/year) + federal grants ($10M/year)
- **Time:** 5 years to build first units, 20 years to reach scale
- **Political:**
  - Support: Renters (40%), socialists, progressives
  - Oppose: Developers (30%), landlords, conservatives
  - Requires: 55% council support (need coalition)

**Effects:**
- Year 5: 10,000 units available, 2% of housing stock
- Year 20: 50,000 units, 10% of stock (rents stabilize)
- Year 50: 200,000 units, 40% of stock (affordable city)
- Mixed-income by design (income cap at 150% median)
- High quality (well-designed, well-maintained)
- **Result: Affordable, stable, diverse neighborhoods**

**Trade-offs:**
- Large upfront investment (hard during budget crisis)
- Developers oppose (compete with them)
- Requires long-term political commitment
- But: Long-term success, moral high ground

---

#### Singapore: Public Housing Homeownership 🏆

**The Context:**
- 1960s: Post-independence, housing crisis, slums
- Government decides: Everyone should own their home
- Builds massive public housing program (HDB - Housing Development Board)

**What They Built:**
- **80% of Singaporeans own their homes** (vs. 65% US)
- **But 75% live in public housing** (government-built)
- High-quality high-rises (20-40 stories)
- Mixed-income towers (rich and poor in same building)
- **Ethnic integration quotas** (prevent segregation by race)

**How It Works:**
- **Government builds and sells** (not rents)
- Subsidized prices (below market, but not free)
- Citizens can buy with **CPF** (mandatory retirement savings)
- **Resale restrictions** (can't flip for profit immediately, must sell back to government first)
- Government owns land (99-year leases)

**Why It Works:**
- **Homeownership creates stability** (people invested in community)
- **Subsidized but not free** (people value what they pay for)
- **Captures land value** (government owns land, leases it)
- **Prevents speculation** (resale restrictions)
- **High quality** (Singaporeans proud of HDB flats, not stigma)
- **Mixed-income = no segregation** (doctor lives next to janitor)

**Results:**
- **Nearly zero homelessness**
- **High homeownership** (even low-income families)
- **Economically integrated** (not segregated by income)
- **Stable communities** (people stay for decades)

**TerminalCity Application:**

**Player Option: Singapore-Style Public Housing Ownership**
- **Cost:** $1B initial investment (build 20,000 units)
- **Funding:** Government bond (paid back by sale proceeds)
- **Sale Price:** $150k/unit (subsidized, but residents pay)
- **Revenue:** $3B over 20 years (as units sell)
- **Net:** Self-funding after initial investment!

**Mechanics:**
- Build towers (high-density, efficient)
- Sell to residents at subsidized rates
- Residents can use savings + mortgage
- Resale restrictions (prevent speculation)
- Mixed-income floors (random assignment)
- **Result: High homeownership, affordable, integrated**

**Trade-offs:**
- Requires land (government must own/acquire)
- Requires scale (doesn't work small)
- Cultural: Some prefer renting flexibility
- But: Long-term self-funding, creates stability

---

#### Tokyo/Japan: Zoning That Allows Housing 🏆

**The Context:**
- Post-WWII: Rapid urbanization, needed housing
- Adopted cumulative zoning system (1968)
- National zoning law (local governments can't block)

**How Zoning Works:**
- **12 zone categories** from most restrictive → most permissive
- **Cumulative:** Each zone allows uses from less-intense zones
- Example:
  - Zone 1: Low-rise residential only
  - Zone 2: Zone 1 + small shops (150 m²)
  - Zone 3: Zones 1-2 + larger shops
  - Zone 4: Zones 1-3 + offices, hotels
  - ...up to Zone 12: Heavy industry

**Key Difference from US:**
- **US:** "This zone is ONLY for single-family homes" (exclusive)
- **Japan:** "This zone allows up to X intensity" (cumulative)
- Result: Small shop in residential area = legal and normal in Japan

**Why It Works:**
- **Supply meets demand** (builders can build where needed)
- **No NIMBY veto power** (national law, local can't block)
- **Gentle density** (missing middle housing allowed)
- **Walkable** (corner stores in residential areas)
- **Affordable:** Tokyo rents stable/falling despite growing population

**Results:**
- **Tokyo: Most affordable major city relative to wages** (median home = 5-6x median income vs. 10-15x in SF/NYC)
- **Housing gets built** (flexible zoning = responsive supply)
- **Walkable neighborhoods** (mixed-use by default)
- **No homelessness crisis** (supply keeps up with demand)

**TerminalCity Application:**

**Player Option: Adopt Japanese-Style Zoning**
- **Cost:** $0 (policy change)
- **Effect:**
  - Allow "missing middle" in single-family zones (duplexes, townhouses, low-rise apartments)
  - Allow corner stores in residential areas
  - Remove parking minimums
- **Political:**
  - Support: YIMBYs (60%), renters, young people, businesses
  - Oppose: NIMBYs (70%), homeowners in single-family areas
  - Requires: Strong political coalition or state preemption

**Timeline:**
- Year 1: Pass zoning reform (political battle)
- Years 2-5: Small changes (corner stores appear, ADUs built)
- Years 6-15: Gradual densification (duplexes replace some single-family)
- Year 20: 20% more housing units, rents stabilize
- **Result: Affordable, walkable, no displacement**

**Trade-offs:**
- NIMBY backlash (fear of change)
- "Neighborhood character" changes (more density)
- Gradual (not instant fix)
- But: No displacement, organic growth, affordable

---

#### Netherlands: Choose Bikes Over Cars 🏆

**The Context:**
- 1970s: Dutch cities car-centric, traffic deaths of children epidemic
- "Stop de Kindermoord" (Stop the Child Murder) protests
- Government decides: Invest in bikes, not more highways

**What They Built:**
- **Physically separated bike lanes** (not just paint)
- Bike parking at train stations (10,000+ spaces)
- Bike-friendly traffic laws (bikes have priority)
- Connected network (can bike across country)

**Why It Works:**
- **Safe** (physical separation = parents let kids bike)
- **Fast** (bikes get priority, direct routes)
- **Integrated with transit** (bike to train station, train to city, bike to destination)
- **Cultural shift** (biking becomes normal, not weird)

**Results:**
- **27% of trips by bike** (vs. 1% in US)
- **Healthier population** (exercise + less pollution)
- **Less car traffic** (people choose bikes)
- **Cheaper than highways** (bike lane costs 1/100th of highway lane)

**TerminalCity Application:**

**Player Option: Protected Bike Infrastructure**
- **Cost:** $50M for comprehensive citywide network
- **Time:** 5 years to build
- **Effect:** Gradual mode shift over 10-20 years
  - Year 5: 5% of trips by bike (early adopters)
  - Year 15: 15% of trips (mainstream)
  - Year 25: 25% of trips (cultural norm)
- **Political:**
  - Support: Environmentalists (70%), young people (60%), health advocates
  - Oppose: Drivers (50% initially, drops to 20% after seeing it work)

**Secondary Effects:**
- Less traffic congestion (bikes take less space)
- Healthier population (reduces healthcare costs)
- Local retail improves (bikes stop more than cars)
- No displacement (use existing streets)

**Trade-offs:**
- Drivers complain (lose lane)
- Doesn't work for long commutes (but most trips are < 5 miles)
- Weather-dependent (though Dutch bike in rain)
- But: Cheap, healthy, sustainable, popular once built

---

#### Barcelona: Superblocks (Reclaim Streets) 🏆

**The Context:**
- 2016: Barcelona overcrowded with cars, pollution bad
- New policy: "Superblocks" - close interior streets to through-traffic

**How It Works:**
- Take 9-block grid (3x3 blocks)
- **Close interior streets to through-traffic** (perimeter stays open)
- Create pedestrian plazas, playgrounds, outdoor dining
- Cars can access (residents, delivery) but can't use as shortcut
- Result: Reclaim streets from cars for people

**Why It Works:**
- **No demolition needed** (just repurpose existing streets)
- **Creates public space** (expensive to buy land, cheap to reclaim asphalt)
- **Popular once implemented** (initial resistance, then loved)
- **Scalable** (can do one at a time)

**Results:**
- **Better air quality** (less traffic)
- **More walking** (pleasant to walk)
- **Local business improves** (outdoor dining, foot traffic)
- **Property values increase** (people want to live there)

**TerminalCity Application:**

**Player Option: Implement Superblocks**
- **Cost:** $500k per superblock (paint, planters, street furniture)
- **Time:** 6 months per superblock
- **Effect:**
  - Interior streets become plazas/parks
  - Local walkability +50%
  - Property values +10% in superblock
  - Traffic on perimeter +15% (cars go around)

**Political:**
- Support: Residents inside superblock (80% after implementation)
- Oppose: Drivers (60% before, 30% after seeing results)
- Pattern: Initial resistance, then loved

**Scaling:**
- Year 1: Pilot 2 superblocks
- Year 5: 10 superblocks (if successful)
- Year 15: 50 superblocks (city-wide transformation)

**Trade-offs:**
- Drivers inconvenienced (must go around)
- Takes space from cars (give to people)
- Initial complaints (always happens)
- But: Popular once built, transforms neighborhoods

---

#### Germany: Housing Co-ops & Tenant Protections 🏆

**The Context:**
- Long tradition of housing cooperatives (1800s+)
- Strong tenant protection laws (1970s+)
- Housing as right, not investment

**How It Works:**

**Housing Cooperatives (Genossenschaften):**
- Non-profit, resident-owned housing
- Members buy share (€5,000-15,000), get lifetime tenancy
- Rent covers costs only (no profit extracted)
- Democratic governance (members vote on decisions)
- Can't be sold for profit (stays co-op forever)

**Tenant Protections:**
- **Rent control:** Based on local average (Mietspiegel)
- **Just cause eviction:** Can't evict without serious reason
- **Long-term tenancies:** Common to rent for decades
- **Tenant unions:** Strong organizing, political power

**Why It Works:**
- **Removes profit motive** (housing not speculation)
- **Stable communities** (long-term residents)
- **Affordable:** Co-op rent 20-40% below market
- **Well-maintained** (residents own it, care about it)

**Results:**
- **Berlin: 15% of housing in co-ops** (stable affordable housing)
- **Munich, Hamburg:** Similar (10-15% co-ops)
- **Low displacement** (long-term tenants protected)
- **Community-owned wealth** (profits stay with residents)

**TerminalCity Application:**

**Player Option: Enable Housing Cooperatives**
- **Cost:** $20M (seed funding for co-op formation)
- **Effect:**
  - Provide low-interest loans to groups forming co-ops
  - Give co-ops priority for city land sales
  - Tax advantages for co-ops
- **Timeline:**
  - Year 5: 5% of new housing is co-ops
  - Year 15: 10% of total housing stock
  - Year 30: 20% of housing (permanent affordability)

**Player Option: Strong Tenant Protections**
- **Cost:** $0 (policy change)
- **Effect:**
  - Rent increases limited to local average +10%
  - Can't evict without just cause
  - Long-term lease security
- **Political:**
  - Support: Renters (70%), tenant unions
  - Oppose: Landlords (80%), real estate industry
  - Requires: Progressive political coalition

**Trade-offs:**
- Less new construction (lower profit margins)
- Landlords complain (less flexibility)
- But: Stable communities, less displacement, more affordable

---

### Funding Mechanisms: How to Pay for Better Options

**The Challenge:** Subway costs $300M, but federal funding only $50M. Need $250M more. Where does it come from?

#### 1. Land Value Tax (Georgist Economics)

**Concept:**
- Tax **land value**, not building value
- Incentivizes density (building more doesn't increase tax)
- Discourages speculation (can't just sit on empty lot)
- Captures value created by public investment (subway increases nearby land values)

**How It Works:**
- Empty lot worth $1M → Tax: $50k/year (5% of land value)
- Same lot with apartment building → Tax: Still $50k/year (building value not taxed)
- Result: Owner builds apartments (makes money, doesn't increase tax)

**Revenue:**
- Replace property tax with land value tax
- Citywide: $500M/year (vs. $400M from property tax)
- Can issue bonds backed by this revenue: $250M bond = $20M/year payment

**Benefits:**
- Incentivizes development (density is free from tax perspective)
- Discourages speculation (holding empty land costs money)
- Captures public value (subway increases land values, tax captures it)
- Can't be evaded (land can't be hidden or moved)

**Political:**
- Support: Economists (90%), urbanists, YIMBYs, housing advocates
- Oppose: Land speculators, owners of vacant lots, parking lot owners
- Neutral: Most homeowners (wash - land tax up, building tax down)

**TerminalCity Application:**
- Player can switch from property tax to land value tax
- Revenue increases 10-20% (more development)
- Vacant lots fill in (speculation discouraged)
- Can fund infrastructure with bonds

---

#### 2. Tax Increment Financing (TIF)

**Concept:**
- Infrastructure increases nearby property values
- Capture future tax increase to pay for infrastructure
- Self-funding: Infrastructure pays for itself

**How It Works:**
1. **Before subway:** Area generates $10M/year property tax
2. **Build subway:** $300M cost (borrow money)
3. **After subway:** Property values double, now $20M/year tax
4. **Increment:** $10M/year increase
5. **Pay bonds:** $10M/year pays off $300M over 30 years

**Benefits:**
- Self-funding (doesn't require tax increase)
- Only works if infrastructure actually creates value (good incentive)
- Captures public value for public benefit

**Risks:**
- Takes 30+ years to pay off
- Assumes property values increase (might not)
- Diverts tax revenue from general fund

**Political:**
- Support: Developers (love infrastructure), urbanists
- Oppose: School districts (lose tax revenue during TIF period)
- Requires: Long-term commitment

**TerminalCity Application:**
- Player can create TIF district along subway line
- Future property value increases pay for subway
- Risk: If property values don't increase enough, shortfall

---

#### 3. Congestion Pricing (Revenue + Behavior Change)

**Concept:**
- Charge drivers to enter downtown (or use highways)
- Reduces traffic AND generates revenue
- Use revenue to fund transit

**How It Works:**
- **London model:** £15/day to drive into central zone (6am-6pm weekdays)
- **Revenue:** £200M/year (London)
- **Effect:** Traffic down 30%, speeds up 20%, air quality better

**Benefits:**
- **Revenue:** $20M/year for typical city
- **Behavior change:** People shift to transit/bikes
- **Less traffic:** Remaining drivers move faster
- **Self-funding:** Revenue funds transit improvements

**Political:**
- **Initial:** Very unpopular (65% oppose)
- **After implementation:** Popular (60% support after seeing results)
- Pattern: "I'll hate this... wait, it's actually better"

**TerminalCity Application:**
- Player can implement congestion pricing
- Initial political backlash (lose votes)
- Year 2-3: People adjust, support increases
- Revenue funds transit expansion ($20M/year)

---

#### 4. Value Capture / Air Rights

**Concept:**
- Public infrastructure creates private value (subway increases property values)
- Sell development rights to capture some of that value
- Developer pays for right to build above subway station

**How It Works:**
1. **City builds subway station:** $30M cost
2. **Nearby land value increases:** $50M increase
3. **Sell air rights:** Developer pays $20M for right to build tower above station
4. **Net cost:** $10M (much cheaper than $30M)

**Benefits:**
- Developers pay for infrastructure they benefit from
- Dense development at transit nodes (good urbanism)
- Public-private partnership

**Trade-offs:**
- Dense development (NIMBYs oppose tall buildings)
- Shares public value with private developer
- Requires strong real estate market

**TerminalCity Application:**
- Player can sell air rights above subway stations
- Developer pays $20M, builds mixed-use tower
- NIMBY opposition (tall building)
- Net: Lower cost for subway, dense TOD

---

#### 5. Municipal Bonds (Borrow and Repay)

**Concept:**
- City borrows money now, pays back over 30 years
- Like mortgage for infrastructure
- Future taxpayers help pay (they benefit too)

**How It Works:**
- Issue $250M bond at 4% interest
- Pay $16M/year for 30 years (total $480M)
- Cost spread over time (affordable annually)

**Benefits:**
- Get infrastructure now (can't wait to save up)
- Future beneficiaries help pay
- Interest rates often low (municipal bonds tax-advantaged)

**Risks:**
- Debt burden on future budgets
- Interest costs money ($230M extra over 30 years)
- Constrains future flexibility

**Political:**
- Often requires voter approval (bond measure)
- Easy if project popular
- Hard during anti-tax sentiment

**TerminalCity Application:**
- Player can issue bonds (voter approval required)
- Success depends on political climate
- Must make annual payments (constrains future budgets)

---

#### 6. Federal/State Grants (Lobby for Money)

**Concept:**
- National government funds local infrastructure
- Requires lobbying, political relationships, grant applications

**How It Works:**
- Apply for infrastructure grant
- Requires matching funds (feds pay 50-80%)
- Competitive (many cities applying)
- Success depends on: Political relationships, project quality, timing

**Benefits:**
- Huge funding (billions available)
- Shares cost with higher levels of government

**Risks:**
- Competitive (might not get it)
- Strings attached (must follow federal rules)
- Timing uncertain (political process slow)

**TerminalCity Application:**
- Player can lobby for federal grants
- Success chance based on: Political connections, project type, current administration
- If successful: Covers 50-80% of costs
- If unsuccessful: Must find other funding

---

#### 7. Public-Private Partnerships (Share Risk and Reward)

**Concept:**
- Private company helps fund infrastructure
- In exchange: Operating rights, revenue share, or development rights

**How It Works:**
- **Build-Operate-Transfer:** Company builds subway, operates for 30 years, then transfers to city
- **Revenue share:** Company gets fare revenue, city gets infrastructure
- **Development rights:** Company gets to build along line

**Benefits:**
- Private capital reduces public cost
- Private expertise in construction/operations
- Risk shared

**Risks:**
- Private profit motive (might cut corners, raise fares)
- Complex contracts (hard to enforce)
- Can go wrong (many PPP failures)

**TerminalCity Application:**
- Player can propose PPP
- Private company funds 40% of subway
- In exchange: 30-year operating contract
- Risk: Company might raise fares, cut service

---

### Combining Funding Sources: The Realistic Approach

**Subway costs $300M. Federal grant: $50M. Need $250M more.**

**Example Funding Package:**
1. **Federal grant:** $50M (free)
2. **Land value tax:** $30M/year revenue → Issue $200M bond
3. **Value capture (air rights):** $40M from developers
4. **TIF:** $10M/year from property value increases → $100M bond
5. **Total:** $50M + $200M + $40M + $100M = $390M (covers cost + overruns)

**This is realistic!** Cities combine funding sources all the time.

**Player Decisions:**
- Which funding sources to use?
- Each has trade-offs (LVT angers land speculators, air rights angers NIMBYs, bonds constrain future budgets)
- Player must build political coalition to pass funding package

---

### Alternative Historical Paths: What If Cities Chose Differently?

**The Premise:** Show what could have been if US cities made different choices in 1950s-1970s.

#### Alternative 1: What If Detroit Built Subways Instead of Highways?

**Historical Reality:**
- 1950s: Detroit builds I-75, I-94, I-375 (highways bisect city)
- Destroys Black Bottom neighborhood (I-375)
- Enables white flight to suburbs
- Downtown hollows out
- 1960s: Race riots, decline accelerates
- 1970s-2000s: Population falls from 1.8M → 700k
- 2013: Bankruptcy

**Alternative History (Player Choice):**

**1950:** Detroit at crossroads
- Federal highway funding available: $100M
- Auto industry powerful (wants highways)
- Black Bottom neighborhood in path

**Player Chooses: Build Subway Instead**
- Cost: $600M (6x highway)
- Funding: Land value tax + federal transit grant + municipal bonds
- Time: 15 years (1950-1965)
- No displacement (underground)

**Timeline:**

**1950-1965:** Subway construction
- 4 lines (50 miles total)
- Connects downtown to neighborhoods
- No highways bisecting city
- Black Bottom neighborhood intact

**1965-1975:** Early effects
- Dense development along subway lines
- Mixed-income neighborhoods (subway accessible)
- Downtown stays vibrant (easy transit access)
- Some white flight still (racial tensions) but less (no highways enabling sprawl)

**1975-1985:** Auto industry decline (still happens)
- Factories close (global competition)
- BUT: City more resilient (diverse, transit-oriented, dense)
- Unemployed auto workers don't leave (affordable, good transit)
- Some economic pain but city stays populated

**1985-2000:** Reinvention
- Tech companies locate downtown (young workers like transit)
- Healthcare expands (Wayne State, hospitals)
- Arts scene thrives (affordable, vibrant neighborhoods)
- Population stabilizes at 1.2M (vs. 700k in reality)

**2020:** Thriving City
- Dense, transit-oriented, economically diverse
- Mixture: Tech, healthcare, education, manufacturing
- Affordable housing (never had massive sprawl/decline cycle)
- **No bankruptcy** (stable tax base)

**The Lesson:**
- Same external shocks (auto industry decline)
- Different infrastructure choices
- Dramatically different outcomes

---

#### Alternative 2: What If Los Angeles Built Transit in 1960s?

**Historical Reality:**
- 1920s-1940s: LA has extensive streetcar network (Pacific Electric "Red Cars")
- 1950s: Streetcars torn up, freeways built
- 1960s-1990s: Car-dependent sprawl, horrific traffic, smog
- 1990s: Finally builds subway/light rail (Metro)
- 2020s: Still car-dominated but improving

**Alternative History:**

**1960:** LA at crossroads
- Pacific Electric dying (cars taking over)
- Federal highway money available
- Smog becoming serious problem

**Player Chooses: Modernize Transit Instead of More Freeways**
- Keep/modernize streetcar lines (convert to light rail)
- Build subway downtown
- Cost: $2B (1960s dollars)
- Funding: Gas tax + federal transit grants + bonds

**Timeline:**

**1960-1975:** Build metro system
- 5 subway lines downtown
- 15 light rail lines to suburbs
- Park-and-ride stations (can't eliminate cars, but reduce)

**1975-1990:** Effects
- Transit ridership: 30% (vs. 5% in reality)
- Less freeway congestion (fewer cars)
- Less sprawl (development concentrates near transit)
- Better air quality (less smog)
- Denser neighborhoods along lines

**1990-2010:** Compound effects
- Transit-oriented development (like real LA doing now, but 30 years earlier)
- Walkable neighborhoods near stations
- Less car dependency (but still cars for long trips)
- Tech companies locate near transit (workers want it)

**2020:** Different City
- Still sprawling (can't change geography) but less car-dependent
- Transit ridership: 35% (vs. 10% in reality)
- Better air quality (50 years less smog)
- More affordable (didn't waste land on parking)
- Healthier (more walking, less driving)

**The Lesson:**
- Can't transform LA to Tokyo (geography different)
- But could have been much better with different choices
- 30 years of compound effects matter

---

#### Alternative 3: What If San Francisco Didn't Block Housing (1970s+)?

**Historical Reality:**
- 1960s-1970s: Neighborhood activists block freeways (good!) but also block housing (bad?)
- 1980s+: Strict zoning, height limits, design review
- Result: Very little housing built, prices skyrocket
- 2020s: Housing crisis, homelessness, inequality

**Alternative History:**

**1975:** San Francisco at crossroads
- Activists successfully stop freeways (hooray!)
- But proposed housing also blocked

**Player Chooses: Stop Freeways But Allow Housing**
- No freeways (keep neighborhoods intact)
- But allow medium-density housing (4-8 stories)
- Allow missing middle (duplexes, townhouses)

**Timeline:**

**1975-1990:** Gradual densification
- 100,000 new housing units (vs. 30,000 in reality)
- Medium-density (not towers, not sprawl)
- Mixed-income (supply keeps up with demand)
- Prices moderate (not cheap, but not crisis)

**1990-2000:** Tech boom (same as reality)
- Tech workers move in
- BUT: Housing supply adequate (prices rise but manageable)
- Less displacement (enough housing for both existing and new residents)

**2000-2020:** Continued growth
- Another 150,000 units (vs. 50,000 in reality)
- Prices high but stable (supply and demand in balance)
- Less homelessness (more affordable)
- More economically diverse (artists, teachers, service workers can still afford it)

**2020:** Still Expensive But Livable
- Median home: $800k (vs. $1.5M in reality)
- Median rent: $2,000 (vs. $3,500 in reality)
- More diverse (not just rich tech workers)
- Less homelessness (more housing)

**The Lesson:**
- Can fight highways AND build housing (both good!)
- Don't have to choose between "preserve everything" and "bulldoze everything"
- Supply matters (economics works)

---

### Gameplay: Letting Players Explore These Paths

**The Design Goal:**
- Don't lecture ("Vienna is better than Detroit")
- Show choices and consequences
- Let players discover through play

**Scenario Mode:**

**Scenario 1: Detroit 1950**
- Historical: What actually happened (highways, white flight, decline)
- Player tries to avoid historical outcome
- Can they do better? (subway, mixed-income housing, etc.)

**Scenario 2: Vienna 1920**
- Build social housing system from scratch
- Manage funding, politics, quality
- Can you create 100 years of affordability?

**Scenario 3: Tokyo 1968**
- Implement cumulative zoning
- Resist NIMBY pressure
- Can you keep housing affordable despite growth?

**Sandbox Mode:**
- Player chooses own path
- Game presents options (highway or subway?)
- Shows historical examples ("Vienna did this, here's how it worked")
- Player experiments: What happens if I...?

**Learning Through Contrast:**

**Playthrough 1:** Player takes easy path (highways, sprawl, displacement)
- Year 50: Segregated, car-dependent, downtown dead
- "Oh no, I created Detroit..."

**Playthrough 2:** Player knows better now
- Choose subway, mixed-income housing, Japanese zoning
- Year 50: Dense, walkable, affordable, vibrant
- "I created Vienna!"

**Playthrough 3:** Player experiments
- Mix approaches, hybrid solutions
- Maybe some highways (freight), but also transit (people)
- Find own balance

**The Meta-Lesson:**
- Better is possible
- It's harder and more expensive
- But long-term outcomes much better
- Real cities made these choices
- Learn from their successes and failures

---

## Emergent Gameplay and the Joy of Observation

### Core Design Principle: Fun to Watch, Not Just Fun to Control

One of the best feelings in city builders is watching your city *live* - setting up conditions, then observing as systems interact and produce emergent, sometimes surprising results. The game should reward patience and observation, not constant intervention.

**The Magic Moment:**
- You make a big decision (build subway, change zoning, adjust budget)
- Speed up time
- Watch the consequences unfold naturally over months/years
- See patterns emerge you didn't explicitly program
- **The city feels alive, not just a collection of buildings you placed**

### What Makes Games Fun to Watch

**Emergence Over Scripting:**
- ❌ **Scripted:** "3 months after subway opens, spawn 5 apartment buildings"
- ✅ **Emergent:** Subway increases land values → Developers respond to profit opportunity → Buildings appear gradually over years → Density emerges naturally

**Systems Interacting:**
- Individual systems are simple (land value, developer behavior, resident preferences)
- Interactions create complexity (patterns you didn't explicitly design)
- Player discovers: "Oh wow, my subway line accidentally created an arts district"

**Temporal Unfolding:**
- Decisions have immediate effects (construction starts)
- Medium-term effects (buildings appear along line)
- Long-term effects (neighborhood character transforms)
- **Watching this unfold is satisfying**

**Visual Feedback:**
- See roads decay over time (budget cuts)
- Watch buildings appear gradually (development)
- See people moving (migration patterns)
- Notice neighborhoods transform (gentrification, decline)
- **The map tells stories without words**

### Time Scales: The Joy of Watching Decades Pass

**TerminalCity should support multiple time scales:**

**Micro (Real-Time to 1 Week/Second):**
- Watch individual workers commute
- See traffic patterns (rush hour pulse)
- Observe construction progress
- **Use for:** Understanding current state, making immediate adjustments

**Meso (1 Month/Second to 1 Year/Second):**
- Watch buildings appear/disappear
- See seasonal cycles (winter damage, spring repair)
- Observe migration flows
- **Use for:** Seeing policy effects, watching neighborhoods change

**Macro (5 Years/Second to 10 Years/Second):**
- Watch decades pass in minutes
- See long-term consequences of decisions
- Observe multiple disruption cycles
- **Use for:** The "SimCity zen" mode - just watch your city evolve

**The "Set It and Watch" Gameplay:**
1. Make big decision (build subway line)
2. Speed up to Macro time (10 years/second)
3. Watch 30 years pass in 3 minutes
4. See neighborhood completely transform
5. Pause, reflect: "That's what my decision did"
6. Make another decision
7. Watch more

### Examples of Emergent Behavior in TerminalCity

#### Example 1: The Accidental Arts District

**Player Action:**
- Year 0: Old factory closes (automation), building sits empty
- Player zones area as mixed-use, doesn't demolish

**Emergent Consequence (Watch It Unfold):**
- Year 2: Artists move in (cheap rent in abandoned factory)
- Year 5: Coffee shop opens nearby (serves artists)
- Year 8: Gallery opens (artists need to show work)
- Year 12: More artists attracted (critical mass forming)
- Year 15: Trendy restaurants open (following the artists)
- Year 18: Property values rising (neighborhood "discovered")
- Year 20: Original artists priced out (gentrification)
- Year 25: Now expensive mixed-use neighborhood

**Player never said "create arts district" - it emerged from:**
- Cheap rent (abandoned factory)
- Zoning permitting mixed-use (artists + businesses)
- Artists attracting businesses
- Businesses attracting more artists
- Positive feedback loop → Arts district
- Then negative feedback loop → Gentrification

**The joy: Watching this story unfold organically over 25 years**

---

#### Example 2: The Highway Death Spiral

**Player Action:**
- Year 0: Build highway through poor neighborhood (cheap option)
- Displace 2,000 residents

**Emergent Consequence:**
- Year 1-5: Highway enables suburban sprawl
  - Families move to suburbs (can commute via highway)
  - New subdivisions appear outside city
  - Property taxes from suburbs go to county, not city

- Year 5-10: Downtown begins decline
  - Customers live in suburbs now (drive to strip malls)
  - Downtown shops lose business
  - Some shops close
  - Visual: Empty storefronts appear

- Year 10-15: Accelerating decline
  - More shops close → Less foot traffic → More shops close
  - Property values drop
  - Crime increases (broken windows theory)
  - Wealthier residents move out

- Year 15-20: Death spiral
  - Tax base eroded (property values down, residents gone)
  - Can't afford services (budget crisis)
  - Cut police/fire/schools
  - Even more people leave
  - Visual: Decay everywhere (potholes, graffiti, abandoned buildings)

- Year 20-30: Stabilization at lower level
  - Population 50% of peak
  - Poor residents remain (can't afford to move)
  - Downtown mostly abandoned
  - Tax revenue collapsed
  - **Detroit scenario - emerged from one highway decision**

**Player never said "create blight and depopulation" - it emerged from:**
- Highway enabling sprawl
- Sprawl pulling customers from downtown
- Downtown decline pushing more people out
- Budget crisis reducing services
- Reduced services pushing more people out
- **Cascading systems interaction**

**The lesson: One decision, decades of compounding consequences**

---

#### Example 3: The Transit-Oriented Success Story

**Player Action:**
- Year 0: Build subway instead of highway (expensive, hard path)
- Takes 10 years to build

**Emergent Consequence:**
- Year 0-10: Construction phase
  - Disruption (construction sites)
  - Some businesses complain
  - Skeptics say "waste of money"

- Year 10-15: First effects
  - Stations open
  - People use transit (faster than bus)
  - Land values increase near stations (accessibility premium)

- Year 15-25: Development boom
  - Developers notice land value increase
  - Old parking lots redeveloped (no longer needed)
  - Apartment buildings appear near stations
  - Ground-floor retail (mixed-use naturally emerges)
  - Walkable neighborhoods form

- Year 25-40: Transformation complete
  - Dense corridors along subway lines
  - Thriving street life (people walking)
  - Local businesses (serve local residents)
  - Property tax revenue increased (density + values)
  - Can afford better services (virtuous cycle)

- Year 40+: Compounding benefits
  - Young professionals choose to live here (like transit)
  - Companies locate near transit (workers want it)
  - Less car dependency (reduced road maintenance costs)
  - Healthier population (more walking)
  - Environmental benefits (less driving)
  - **Vienna/Tokyo scenario - emerged from transit investment**

**Player never said "create dense walkable neighborhoods" - it emerged from:**
- Transit increasing land values
- Developers responding to profit opportunity
- Density attracting more transit riders
- Transit enabling walkable lifestyle
- Walkability attracting people who value it
- **Positive feedback loops**

**The satisfaction: Watching the hard choice pay off over decades**

---

#### Example 4: The Zoning Cascade

**Player Action:**
- Year 0: Adopt Japanese-style cumulative zoning (allow missing middle housing)

**Emergent Consequence:**
- Year 1-5: Slow start
  - Few visible changes (takes time)
  - Some ADUs built in backyards
  - Occasional duplex replaces single-family
  - NIMBYs complain but changes small

- Year 5-15: Gradual densification
  - More duplexes, townhouses appear
  - Corner stores open in residential areas (now legal)
  - Walking increases (shops within walking distance)
  - Transit becomes more viable (density supports it)

- Year 15-30: Compounding effects
  - Increased density → Better transit service (more riders)
  - Better transit → More people can live without cars
  - Less parking needed → Land available for housing
  - More housing → Rents stabilize
  - Stable rents → Diverse population can afford to stay
  - Diverse population → Vibrant neighborhoods

- Year 30-50: Mature system
  - Gentle density throughout (not towers, not sprawl)
  - Mixed-use naturally (shops + housing)
  - Affordable (supply kept pace with demand)
  - Walkable (everything nearby)
  - Economically diverse (housing at multiple price points)
  - **Tokyo scenario - emerged from flexible zoning**

**Player never said "create affordable walkable city" - it emerged from:**
- Zoning allowing flexibility
- Developers responding to demand
- Increased supply keeping prices stable
- Density enabling walkability
- Walkability attracting people who value it
- **System enabling organic adaptation**

**The payoff: Patient investment in good policy pays off over 50 years**

---

### Visual Feedback That Makes Observation Satisfying

**Roads:**
- Year 0: Pristine (bright colors, clean lines: ═══)
- Year 5 (underfunded): Minor cracks appear
- Year 10: Potholes visible (character decay: ═▓═)
- Year 15: Severe decay (░▓░)
- **Watch deterioration happen gradually**

**Buildings:**
- Well-maintained: Bright colors, clean appearance
- Neglected: Colors fade to gray
- Decaying: Characters corrupt (⌂ → ╫ → ≈)
- Abandoned: Heavy distortion, dark colors
- Demolished: Empty lot (.)
- New construction: Scaffolding (▒), then new building
- **Watch neighborhoods age and transform**

**Neighborhoods:**
- Thriving: Bright, varied colors, busy streets, dense
- Stable: Normal appearance, moderate activity
- Declining: Faded colors, empty buildings, sparse
- Abandoned: Dark, decayed, few lights at night
- Gentrifying: Mix of old decay and new construction
- **Map shows neighborhood health at a glance**

**Traffic:**
- Rush hour: Thick flow lines on major routes
- Midday: Light traffic
- Night: Sparse
- Congestion: Red routes (backed up)
- Transit: People flowing to stations
- **Living pulse of the city**

**Seasons:**
- Spring: Green parks, fresh appearance, construction starts
- Summer: Bright, active, lots of outdoor activity
- Fall: Color changes, harvest (if agriculture)
- Winter: Snow accumulation, road damage, heating visible
- **Annual cycle creates rhythm**

**Long-term Changes:**
- Population density: See neighborhoods fill in or empty out
- Building heights: Watch downtown skyline grow
- Land use: See zones transform (industrial → residential)
- Infrastructure: Watch subway network expand
- **Decades of change visible on map**

### The "SimCity Zen" Mode

**How It Feels:**
1. **Set up systems** (zoning, budgets, policies)
2. **Speed up time** (5-10 years/second)
3. **Just watch:**
   - Buildings appearing and disappearing
   - Neighborhoods transforming
   - People migrating
   - Traffic patterns shifting
   - Seasons cycling
   - Decades passing
4. **Notice patterns:**
   - "Oh, that neighborhood is gentrifying"
   - "The subway line created a dense corridor"
   - "My budget cuts are causing visible decay"
   - "That policy had unintended consequences"
5. **Pause when something interesting happens**
6. **Make adjustment if needed**
7. **Watch more**

**The Meditative Quality:**
- Not frantic (not micromanaging)
- Not idle (systems constantly changing)
- Observational (watching complexity unfold)
- Satisfying (seeing long-term consequences)
- Educational (learning how systems interact)

**Like watching:**
- Ant farm (complex behavior from simple rules)
- Aquarium (living ecosystem)
- Garden growing (patience rewarded)
- Time-lapse of city (decades in minutes)

### Replayability Through Emergence

**Same Decisions, Different Contexts:**

**Playthrough 1: Build subway in Year 0**
- City young, mostly single-family
- Subway enables densification
- Result: Dense transit corridors emerge

**Playthrough 2: Build subway in Year 30**
- City already sprawling, car-dependent
- Harder to change established patterns
- Result: Some density, but less transformative

**Playthrough 3: Build subway in Year 60**
- City declining, population leaving
- Subway seen as waste of money by some
- Result: Can enable revival, but struggling

**Same action, different emergent outcomes based on context**

**Butterfly Effects:**

Small changes create divergent futures:
- **With subway:** Dense, walkable, low car ownership, thriving downtown, diverse neighborhoods
- **Without subway:** Sprawling, car-dependent, dead downtown, segregated suburbs
- Both emerged from one decision in Year 0

**Learning Through Observation:**
- First playthrough: "Interesting, I created X"
- Second playthrough: "What if I change Y?"
- Third playthrough: "I wonder what happens if..."
- Endless experimentation with emergent results

### Systems That Should Interact

**Land Values:**
- Affected by: Transit access, school quality, crime, services, neighborhood quality
- Affects: Development pressure, tax revenue, who can afford to live there

**Development Pressure:**
- Affected by: Land values, zoning, profitability
- Affects: What gets built, density, neighborhood character

**Population:**
- Affected by: Jobs, housing costs, quality of life, services
- Affects: Demand for housing, tax revenue, labor supply

**Employment:**
- Affected by: Economic changes, automation, industry shifts
- Affects: Income, migration, tax revenue, social stability

**Transit:**
- Affected by: Funding, density, coverage
- Affects: Land values, car dependency, development patterns

**Services:**
- Affected by: Budget, coverage, staffing
- Affects: Quality of life, property values, political support

**Crime:**
- Affected by: Poverty, police funding, neighborhood quality, employment
- Affects: Property values, migration, business viability

**Political Support:**
- Affected by: Service quality, taxes, policy choices, economic conditions
- Affects: What policies you can pass, whether you get reelected

**These systems constantly influence each other:**
- Not simple cause-effect
- Feedback loops (positive and negative)
- Delayed effects (years to see results)
- Emergent patterns
- **Complex but comprehensible**

### Design Principles for Emergent Gameplay

**1. Simple Rules, Complex Outcomes:**
- Each system has simple logic (land values increase near good transit)
- Interactions create complexity (transit → values → development → density → more transit demand)
- Player can understand individual systems but outcomes surprise

**2. Feedback Loops:**
- **Positive:** Success breeds success (good schools → wealthy families → more tax revenue → better schools)
- **Negative:** Decline accelerates (crime → people leave → less tax revenue → cut police → more crime)
- Player must identify and manage loops

**3. Time Delays:**
- Decisions don't have instant effects
- Subway takes 10 years to build, 20 years to transform neighborhood
- Forces patience, rewards long-term thinking
- Creates anticipation (what will happen?)

**4. Spatial Patterns:**
- Effects radiate outward (subway station affects nearby blocks most)
- Creates gradients (very dense near station, less dense farther)
- Visual patterns emerge (corridors, clusters, rings)

**5. Multiple Equilibria:**
- City can stabilize at different states (thriving, declining, mixed)
- Hard to move between states (hysteresis)
- Policy can push toward different equilibria
- No single "correct" state

**6. Player is Facilitator, Not Controller:**
- Set conditions (zoning, budgets, infrastructure)
- Systems execute (developers build, people move, businesses open)
- Player observes outcomes (did it work?)
- Adjust as needed (iterate)
- **Guide, don't micromanage**

### The Satisfaction of Watching Consequences Unfold

**Immediate Gratification (Seconds to Minutes):**
- Place building, see it appear
- Adjust budget, see number change
- Pass ordinance, get political reaction

**Medium-Term Discovery (Months to Years):**
- Watch buildings appear along new transit line
- See neighborhood begin to gentrify
- Notice crime increasing in underfunded area

**Long-Term Revelation (Decades):**
- "Oh, that highway decision 30 years ago created this segregated city"
- "That transit investment took 20 years but totally transformed the city"
- "My zoning reform gradually made housing affordable"

**Aha Moments:**
- "I see why malls killed downtown now" (watching it happen)
- "Oh, land value tax really does encourage development" (seeing vacant lots fill in)
- "Wow, budget cuts create death spirals" (watching neighborhood decline)
- **Learning by observing, not being told**

**The Joy:**
- Making decisions
- Waiting (anticipation)
- Watching unfold (discovery)
- Understanding (insight)
- Reflecting ("that's why that happened")
- Experimenting ("what if I did it differently?")
- **Endless cycle of learning and discovery**

### Implementation: Making It Watchable

**Performance:**
- Must run fast even at high time speed (10 years/second)
- Optimize for many small updates, not few large updates
- Can't recalculate everything every frame

**Visual Clarity:**
- Changes should be visible at macro level (zoomed out)
- Color coding (green = thriving, red = declining)
- Animation (buildings appearing, traffic flowing)
- Indicators (up/down arrows for trends)

**Information Overlays:**
- Can toggle views: Land value, crime, density, employment
- See spatial patterns clearly
- Understand why things are happening
- **But default view should tell story visually without overlays**

**Time Controls:**
- Pause, 1x, 5x, 10x, 50x, 100x speeds
- Can skip ahead (jump 10 years)
- Can review history (rewind to see what changed)
- Smooth speed changes (no jarring jumps)

**The Goal:**
- Should be hypnotic to watch
- Like watching waves, fire, falling snow
- But also informative
- **Beautiful, alive, meaningful**

---

*These notes capture thinking-in-progress. They inform design decisions but don't represent finalized features.*
